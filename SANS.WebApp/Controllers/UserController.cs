#region 版权声明
/**************************************************************** 
 * 作    者：周黎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2018/6/8 21:41:11 
 * 当前版本：1.0.0.1 
 *  
 * 描述说明： 
 * 
 * 修改历史： 
 * 
***************************************************************** 
 * Copyright @ ZhouLi 2018 All rights reserved 
 *┌──────────────────────────────────┐
 *│　此技术信息周黎的机密信息，未经本人书面同意禁止向第三方披露．　│
 *│　版权所有：周黎 　　　　　　　　　　　　　　│
 *└──────────────────────────────────┘
*****************************************************************/
#endregion
using System.Linq;
using DInjectionProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SANS.BLL;
using SANS.BLL.Interface;
using SANS.Common;
using SANS.DbEntity.Models;
using SANS.WebApp.Data;
using SANS.WebApp.Filters;
using SANS.WebApp.Models;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using SANS.WebApp.Comm;
using SANS.Log;

namespace SANS.WebApp.Controllers
{
    public class UserController : Controller
    {
        private WholeInjection injection;
        private IHostingEnvironment hostingEnvironment;
        public UserController(WholeInjection injection, IHostingEnvironment env)
        {
            this.injection = injection;
            this.hostingEnvironment = env;
        }

        public IActionResult Login()
        {
            ViewBag.SysName = ProjectConfig.SysName;
            return View();
        }
        public IActionResult UserInfo()
        {
            ViewBag.UserInfo = AutoMapper.Mapper.Map<SysUserDto>(injection.GetT<ISysUserBLL>().GetModels(t => t.UserId.Equals(injection.GetT<UserAccount>().GetUserInfo().UserId)).SingleOrDefault());
            return View();
        }
        public IActionResult UserChagePwd() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]//防伪标记 预防坏蛋攻击
        public string UserChagePwd(string useroldpwd, string usernewpwd)
        {
            var response = new ResponseModel();
            var userlogin = injection.GetT<UserAccount>().GetUserInfo();
            if (!userlogin.UserPwd.Equals(MD5Encrypt.Get32MD5One(useroldpwd)))
            {
                response.Messages = "原密码不正确,请重新输入";
                response.StateCode = StatesCode.failure;
                return JsonHelper.ObjectToJson(response);
            }
            var user = injection.GetT<ISysUserBLL>().GetModels(t => t.UserId.Equals(userlogin.UserId)).SingleOrDefault();
            if (user != null)
            {
                user.UserPwd = MD5Encrypt.Get32MD5One(usernewpwd);
                if (injection.GetT<ISysUserBLL>().Update(user))
                {
                    response.Messages = "密码修改成功";
                    response.StateCode = StatesCode.success;
                    //密码修改成功 重新登录
                    injection.GetT<UserAccount>().Login(user);
                }
                else
                {
                    response.Messages = "密码修改失败";
                    response.StateCode = StatesCode.failure;
                }
            }
            else
            {
                response.Messages = "账户不存在,请联系管理员!";
                response.StateCode = StatesCode.failure;
            }
            return JsonHelper.ObjectToJson(response);
        }
        /// <summary>
        /// 后台登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]//防伪标记 预防坏蛋攻击
        public IActionResult UserLogin(string username, string password)
        {

            var message = new ResponseModel();
            try
            {
                var sysUsers = injection.GetT<ISysUserBLL>().GetModelNoTracking(t =>
                    (t.UserName.Equals(username) ||
                    t.UserEmail.Equals(username) ||
                    t.UserPhone.Equals(username) && t.DeleteSign.Equals(1)));
                if (sysUsers == null)
                {
                    message.StateCode = StatesCode.failure;
                    message.Messages = "该账户不存在";
                }
                else
                {
                    if (sysUsers.UserStatus.Equals((int)SysEnum.Enum_UserStatus.Status_Discontinuation))
                    {
                        message.StateCode = StatesCode.failure;
                        message.Messages = "账户已停用,请联系管理员解除";
                    }
                    else if (!MD5Encrypt.Get32MD5Two(password).Equals(sysUsers.UserPwd))
                    {
                        message.StateCode = StatesCode.failure;
                        message.Messages = "密码错误";
                    }
                    else
                    {
                        var user = injection.GetT<ISysUserBLL>().GetLoginSysUser(sysUsers).Data;
                        injection.GetT<UserAccount>().Login(user);
                        message.Messages = "登陆成功";
                        message.JsonData = new { baseUrl = "/Home/Index", UserId = sysUsers.UserId, tokenUuid = Guid.NewGuid().ToString() };
                    }

                }
            }
            catch (Exception er)
            {
                Log4netHelper.Error(this, er);
            }
            return Json(message);
        }
        [HttpPost]
        public ActionResult UploadImg()
        {
            var userlogin = injection.GetT<UserAccount>().GetUserInfo();
            if (Request.Form.Files.Count > 0 && userlogin != null)
            {
                //p1,p2没什么用，只是为了证明前端中额外参数data{parm1,parm2}成功传到后台了
                //string p1 = Parm1;
                //string p2 = Parm2;
                //获取后缀名
                string ext = Path.GetExtension(Request.Form.Files[0].FileName);
                //获取/upload/文件夹的物理路径
                string mapPath = @hostingEnvironment.WebRootPath;
                //通过上传时间来创建文件夹，每天的放在一个文件夹中
                string imgPath = "/upload";
                string dir = mapPath + imgPath;
                if (!Directory.Exists(dir))
                {
                    DirectoryInfo dirInfo = Directory.CreateDirectory(dir);
                }
                //获取图片的相对路径
                string imgSrc = imgPath + "/" + Guid.NewGuid().ToString() + ext;
                //在服务器存储文件，文件名为一个GUID
                string fullPath = mapPath + imgSrc;
                using (FileStream fs = System.IO.File.Create(fullPath))
                {
                    Request.Form.Files[0].CopyTo(fs);
                    fs.Flush();
                }
                var user = injection.GetT<ISysUserBLL>().GetModel(t => t.UserId.Equals(userlogin.UserId));
                if (user != null)
                {
                    user.UserAvatar = imgSrc;
                    injection.GetT<ISysUserBLL>().Update(user);
                }
                return Json(new { IsSuccess = 1, Msg = "上传成功", Src = imgSrc });
            }
            else
            {
                return Json(new { IsSuccess = 0, Msg = "上传失败", Src = "" });
            }
        }
    }
}