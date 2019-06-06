
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SANS.DbEntity.Models;

namespace SANS.BLL
{

    /// <summary>
    /// model转换dto配置
    /// </summary>
    public class MyMapper
    {
        /// <summary>
        /// 配置DTO映射关系
        /// </summary>
        public static void Initialize()
        {
            //利用反射动态映射
            Mapper.Initialize(MappingAutoMapper);
        }
        private static void MappingAutoMapper(IMapperConfigurationExpression cfg)
        {
            string[] assemblyNames = { "SANS.BLL", "SANS.DbEntity" };
            //先找出Model所有实体类
            List<Type> modelTypes = Assembly.Load("SANS.DbEntity").GetTypes().Where(t => t.Namespace.Equals("SANS.DbEntity.Models")).ToList();
            //model对应的Dto实体(注意:这里我做了命名约定,所有对应实体的Dto对象都以Dto结尾)
            List<Type> modelDtoTypes = Assembly.Load("SANS.BLL").GetTypes().Where(t => t.Name.EndsWith("Dto")).ToList();
            foreach (var dtoType in modelDtoTypes)
            {
                var modelType = modelTypes.Where(t => t.Name.StartsWith(dtoType.Name.Replace("Dto", ""))).First();
                cfg.CreateMap(dtoType, modelType);
                cfg.CreateMap(modelType, dtoType);
            }
        }
    }
}
