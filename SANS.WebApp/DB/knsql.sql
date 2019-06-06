--
-- 由SQLiteStudio v3.2.1 产生的文件 周三 11月 28 11:39:58 2018
--
-- 文本编码：UTF-8
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- 表：Dict_AuthorityType
CREATE TABLE Dict_AuthorityType (
    AuthorityTypeId   INTEGER       PRIMARY KEY AUTOINCREMENT
                                    NOT NULL,
    AuthorityTypeName NVARCHAR (20) NOT NULL
                                    COLLATE NOCASE
);

INSERT INTO Dict_AuthorityType (
                                   AuthorityTypeId,
                                   AuthorityTypeName
                               )
                               VALUES (
                                   1,
                                   '菜单权限'
                               );


-- 表：Sys_AmRelated
CREATE TABLE Sys_AmRelated (
    AmRelatedId VARCHAR (40) NOT NULL,
    AuthorityId VARCHAR (40) NOT NULL,
    MenuId      VARCHAR (40) NOT NULL,
    PRIMARY KEY (
        AmRelatedId
    )
);

INSERT INTO Sys_AmRelated (
                              AmRelatedId,
                              AuthorityId,
                              MenuId
                          )
                          VALUES (
                              '7dad7519-8bf2-4783-a726-1102716c57b7',
                              '96754cd1-1cea-44bb-850a-fdec57c319d5',
                              'ee650cba-5c5b-4ca3-8b8a-208b2c8587bf'
                          );

INSERT INTO Sys_AmRelated (
                              AmRelatedId,
                              AuthorityId,
                              MenuId
                          )
                          VALUES (
                              '210b2fd5-f252-4588-bda1-342b35093d9a',
                              '0ab939db-843e-4e0e-85f5-f367fe66b80d',
                              '5d872e09-1c38-4488-8fe6-a3d5748882bf'
                          );

INSERT INTO Sys_AmRelated (
                              AmRelatedId,
                              AuthorityId,
                              MenuId
                          )
                          VALUES (
                              '06f331ab-0b29-46af-8b2f-b88d5f7b63ad',
                              '3dde90cc-722a-4479-9703-dec9376eeee9',
                              '075a127d-1934-43c4-a4e7-8ca0c56b5ca2'
                          );

INSERT INTO Sys_AmRelated (
                              AmRelatedId,
                              AuthorityId,
                              MenuId
                          )
                          VALUES (
                              'c6924744-70d4-4003-8a5f-f92d97fd74bb',
                              'a714f31d-38d8-45f4-8d15-5e311d96d615',
                              'b4e8ae67-c12f-4f85-a117-6d9ede95f8b3'
                          );

INSERT INTO Sys_AmRelated (
                              AmRelatedId,
                              AuthorityId,
                              MenuId
                          )
                          VALUES (
                              'cb838871-e433-4f9c-b329-3a28922c51c4',
                              '525d6319-7d98-4b70-b344-385266ae1dfc',
                              'ff9d20dc-1eb1-4696-8e1f-d688874d16f3'
                          );

INSERT INTO Sys_AmRelated (
                              AmRelatedId,
                              AuthorityId,
                              MenuId
                          )
                          VALUES (
                              '6570fd9c-25b2-4bf1-8114-11bc8df4e5ac',
                              '1ba84dc8-88a3-48c2-8fce-fbe1c61cf700',
                              'fdad825e-2468-4970-8621-d39f562df076'
                          );

INSERT INTO Sys_AmRelated (
                              AmRelatedId,
                              AuthorityId,
                              MenuId
                          )
                          VALUES (
                              'c0f1edc3-1088-4908-886b-2cc0e2adb327',
                              '7018423c-521f-43f9-b3bb-86584e6a32ee',
                              '2b568275-7552-40d3-85b8-bbb171d9aa58'
                          );

INSERT INTO Sys_AmRelated (
                              AmRelatedId,
                              AuthorityId,
                              MenuId
                          )
                          VALUES (
                              '2d247e8b-d5a4-48a7-aeff-492aecb305e6',
                              'fbae8914-4ebb-4751-bf9b-9eb109016efd',
                              'fd28c979-e2b8-4b66-8f9e-ab8324cc2ec6'
                          );


-- 表：Sys_Authority
CREATE TABLE Sys_Authority (
    AuthorityId   VARCHAR (40)    NOT NULL,
    AuthorityType INTEGER         NOT NULL,
    CreateUserId  VARCHAR (40),
    CreateTime    DATETIME        NOT NULL
                                  DEFAULT (CURRENT_TIMESTAMP),
    EditTime      DATETIME,
    DeleteSign    INTEGER         NOT NULL
                                  DEFAULT 1,
    DeleteTime    DATETIME,
    Note          NVARCHAR (2048) COLLATE NOCASE,
    PRIMARY KEY (
        AuthorityId
    )
);

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              'a714f31d-38d8-45f4-8d15-5e311d96d615',
                              1,
                              NULL,
                              '2018-11-20 13:08:03',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '183dea2d-5dc8-4e22-a56e-98f210149384',
                              1,
                              NULL,
                              '2018-11-20 13:08:03',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              'e115bcbe-68a0-442e-a84f-acc3dee12c18',
                              1,
                              NULL,
                              '2018-11-20 13:08:03',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '3dde90cc-722a-4479-9703-dec9376eeee9',
                              1,
                              NULL,
                              '2018-11-20 13:08:03',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '0ab939db-843e-4e0e-85f5-f367fe66b80d',
                              1,
                              NULL,
                              '2018-11-20 13:08:03',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '96754cd1-1cea-44bb-850a-fdec57c319d5',
                              1,
                              NULL,
                              '2018-11-20 13:08:03',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '525d6319-7d98-4b70-b344-385266ae1dfc',
                              1,
                              NULL,
                              '2018-11-21 06:59:21',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '1ba84dc8-88a3-48c2-8fce-fbe1c61cf700',
                              1,
                              NULL,
                              '2018-11-21 07:01:11',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '7018423c-521f-43f9-b3bb-86584e6a32ee',
                              1,
                              NULL,
                              '2018-11-27 06:55:52',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_Authority (
                              AuthorityId,
                              AuthorityType,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              'fbae8914-4ebb-4751-bf9b-9eb109016efd',
                              1,
                              NULL,
                              '2018-11-27 06:56:24',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );


-- 表：Sys_DataDictionary
CREATE TABLE Sys_DataDictionary (
    dict_id     VARCHAR (40)  PRIMARY KEY,
    type_code   VARCHAR (40),
    type_name   VARCHAR (100),
    code        VARCHAR (40),
    name        VARCHAR (100),
    code_value  VARCHAR (200),
    code_sort   INTEGER       NOT NULL,
    creat_time  DATETIME,
    parent_code VARCHAR (40),
    remark      VARCHAR (100) 
);

INSERT INTO Sys_DataDictionary (
                                   dict_id,
                                   type_code,
                                   type_name,
                                   code,
                                   name,
                                   code_value,
                                   code_sort,
                                   creat_time,
                                   parent_code,
                                   remark
                               )
                               VALUES (
                                   '1',
                                   'NetMQ',
                                   'NetMQ队列配置',
                                   'UrlPublisherServePubString',
                                   '订阅地址',
                                   'tcp://127.0.0.1:6556',
                                   0,
                                   NULL,
                                   NULL,
                                   NULL
                               );

INSERT INTO Sys_DataDictionary (
                                   dict_id,
                                   type_code,
                                   type_name,
                                   code,
                                   name,
                                   code_value,
                                   code_sort,
                                   creat_time,
                                   parent_code,
                                   remark
                               )
                               VALUES (
                                   '4c45e3dc-7627-4909-884d-3278a3d29586',
                                   'NetMQ',
                                   'NetMQ队列',
                                   'URL1',
                                   '测试名称',
                                   '127.0.0.1',
                                   2,
                                   '2018-11-28 09:29:31.4489838',
                                   NULL,
                                   NULL
                               );


-- 表：Sys_Menu
CREATE TABLE Sys_Menu (
    MenuId       VARCHAR (40)    NOT NULL,
    MenuName     NVARCHAR (50)   NOT NULL
                                 COLLATE NOCASE,
    MenuIcon     NVARCHAR (50)   COLLATE NOCASE,
    MenuUrl      VARCHAR (80)    COLLATE NOCASE,
    MenuSort     INTEGER         NOT NULL,
    ParentMenuId VARCHAR (40),
    CreateUserId VARCHAR (40),
    CreateTime   DATETIME        NOT NULL
                                 DEFAULT (CURRENT_TIMESTAMP),
    EditTime     DATETIME,
    DeleteSign   INTEGER         NOT NULL
                                 DEFAULT 1,
    DeleteTime   DATETIME,
    Note         NVARCHAR (2048) COLLATE NOCASE,
    PRIMARY KEY (
        MenuId
    )
);

INSERT INTO Sys_Menu (
                         MenuId,
                         MenuName,
                         MenuIcon,
                         MenuUrl,
                         MenuSort,
                         ParentMenuId,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'ee650cba-5c5b-4ca3-8b8a-208b2c8587bf',
                         '菜单管理',
                         NULL,
                         '/System/Menu/Index',
                         0,
                         'b4e8ae67-c12f-4f85-a117-6d9ede95f8b3',
                         NULL,
                         '2018-11-20 13:08:03',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_Menu (
                         MenuId,
                         MenuName,
                         MenuIcon,
                         MenuUrl,
                         MenuSort,
                         ParentMenuId,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'b4e8ae67-c12f-4f85-a117-6d9ede95f8b3',
                         '系统管理',
                         NULL,
                         NULL,
                         1,
                         '00000000-0000-0000-0000-000000000000',
                         NULL,
                         '2018-11-20 13:08:03',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_Menu (
                         MenuId,
                         MenuName,
                         MenuIcon,
                         MenuUrl,
                         MenuSort,
                         ParentMenuId,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '075a127d-1934-43c4-a4e7-8ca0c56b5ca2',
                         '角色管理',
                         NULL,
                         '/System/Role/Index',
                         0,
                         'b4e8ae67-c12f-4f85-a117-6d9ede95f8b3',
                         NULL,
                         '2018-11-20 13:08:03',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_Menu (
                         MenuId,
                         MenuName,
                         MenuIcon,
                         MenuUrl,
                         MenuSort,
                         ParentMenuId,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '5d872e09-1c38-4488-8fe6-a3d5748882bf',
                         '用户管理',
                         NULL,
                         '/System/User/Index',
                         0,
                         'b4e8ae67-c12f-4f85-a117-6d9ede95f8b3',
                         NULL,
                         '2018-11-20 13:08:03',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_Menu (
                         MenuId,
                         MenuName,
                         MenuIcon,
                         MenuUrl,
                         MenuSort,
                         ParentMenuId,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'ff9d20dc-1eb1-4696-8e1f-d688874d16f3',
                         '业务管理',
                         NULL,
                         NULL,
                         0,
                         '00000000-0000-0000-0000-000000000000',
                         NULL,
                         '2018-11-21 06:59:21',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_Menu (
                         MenuId,
                         MenuName,
                         MenuIcon,
                         MenuUrl,
                         MenuSort,
                         ParentMenuId,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'fdad825e-2468-4970-8621-d39f562df076',
                         '检测管理',
                         NULL,
                         '/Business/RealtimeMonitoring/Index',
                         0,
                         'ff9d20dc-1eb1-4696-8e1f-d688874d16f3',
                         NULL,
                         '2018-11-21 07:01:11',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_Menu (
                         MenuId,
                         MenuName,
                         MenuIcon,
                         MenuUrl,
                         MenuSort,
                         ParentMenuId,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '2b568275-7552-40d3-85b8-bbb171d9aa58',
                         '组合参数',
                         NULL,
                         NULL,
                         0,
                         'ff9d20dc-1eb1-4696-8e1f-d688874d16f3',
                         NULL,
                         '2018-11-27 06:55:53',
                         NULL,
                         2,
                         '2018-11-27 16:36:32.4488046',
                         NULL
                     );

INSERT INTO Sys_Menu (
                         MenuId,
                         MenuName,
                         MenuIcon,
                         MenuUrl,
                         MenuSort,
                         ParentMenuId,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'fd28c979-e2b8-4b66-8f9e-ab8324cc2ec6',
                         '系统参数管理',
                         NULL,
                         '/System/Dict/Index',
                         0,
                         'b4e8ae67-c12f-4f85-a117-6d9ede95f8b3',
                         NULL,
                         '2018-11-27 06:56:25',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );


-- 表：Sys_RaRelated
CREATE TABLE Sys_RaRelated (
    RaRelatedId VARCHAR (40) NOT NULL,
    RoleId      VARCHAR (40) NOT NULL,
    AuthorityId VARCHAR (40) NOT NULL,
    PRIMARY KEY (
        RaRelatedId
    )
);

INSERT INTO Sys_RaRelated (
                              RaRelatedId,
                              RoleId,
                              AuthorityId
                          )
                          VALUES (
                              '719c3aa9-f1f7-4d3d-bb41-372bf0454c36',
                              '05337548-9da5-4e35-a3fd-35f515305445',
                              '96754cd1-1cea-44bb-850a-fdec57c319d5'
                          );

INSERT INTO Sys_RaRelated (
                              RaRelatedId,
                              RoleId,
                              AuthorityId
                          )
                          VALUES (
                              '9c1ff5d8-a0ae-463d-9bd2-924c4770a3a0',
                              '05337548-9da5-4e35-a3fd-35f515305445',
                              '0ab939db-843e-4e0e-85f5-f367fe66b80d'
                          );

INSERT INTO Sys_RaRelated (
                              RaRelatedId,
                              RoleId,
                              AuthorityId
                          )
                          VALUES (
                              'ad151037-2112-4459-955a-b2033f08e22e',
                              '05337548-9da5-4e35-a3fd-35f515305445',
                              'e115bcbe-68a0-442e-a84f-acc3dee12c18'
                          );

INSERT INTO Sys_RaRelated (
                              RaRelatedId,
                              RoleId,
                              AuthorityId
                          )
                          VALUES (
                              '7715138d-278f-4705-806e-e47f7c9bd7e0',
                              '05337548-9da5-4e35-a3fd-35f515305445',
                              '3dde90cc-722a-4479-9703-dec9376eeee9'
                          );

INSERT INTO Sys_RaRelated (
                              RaRelatedId,
                              RoleId,
                              AuthorityId
                          )
                          VALUES (
                              '85829794-f2e9-47e8-8cc3-c52fcfcd3046',
                              '05337548-9da5-4e35-a3fd-35f515305445',
                              'a714f31d-38d8-45f4-8d15-5e311d96d615'
                          );


-- 表：Sys_Role
CREATE TABLE Sys_Role (
    RoleId       VARCHAR (40)    NOT NULL,
    RoleName     NVARCHAR (50)   NOT NULL
                                 COLLATE NOCASE,
    CreateUserId VARCHAR (40),
    CreateTime   DATETIME        NOT NULL
                                 DEFAULT (CURRENT_TIMESTAMP),
    EditTime     DATETIME,
    DeleteSign   INTEGER         NOT NULL
                                 DEFAULT 1,
    DeleteTime   DATETIME,
    Note         NVARCHAR (2048) COLLATE NOCASE,
    PRIMARY KEY (
        RoleId
    )
);

INSERT INTO Sys_Role (
                         RoleId,
                         RoleName,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '05337548-9da5-4e35-a3fd-35f515305445',
                         '管理员',
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 05:20:32',
                         '2018-11-21 13:35:59.9574998',
                         1,
                         NULL,
                         NULL
                     );


-- 表：Sys_UgrRelated
CREATE TABLE Sys_UgrRelated (
    UgrRelatedId VARCHAR (40) NOT NULL,
    UserGroupId  VARCHAR (40) NOT NULL,
    RoleId       VARCHAR (40) NOT NULL,
    PRIMARY KEY (
        UgrRelatedId
    )
);


-- 表：Sys_UrRelated
CREATE TABLE Sys_UrRelated (
    UrRelatedId VARCHAR (40) NOT NULL,
    UserId      VARCHAR (40) NOT NULL,
    RoleId      VARCHAR (40) NOT NULL,
    PRIMARY KEY (
        UrRelatedId
    )
);

INSERT INTO Sys_UrRelated (
                              UrRelatedId,
                              UserId,
                              RoleId
                          )
                          VALUES (
                              '802ef608-f18d-4d6c-a6d6-3f044d260f7b',
                              '41faaf517168485584555989d76176c6',
                              '05337548-9da5-4e35-a3fd-35f515305445'
                          );


-- 表：Sys_User
CREATE TABLE Sys_User (
    UserId       VARCHAR (40)    NOT NULL,
    UserName     NVARCHAR (20)   NOT NULL
                                 COLLATE NOCASE,
    UserNikeName NVARCHAR (20)   COLLATE NOCASE,
    UserPwd      NVARCHAR (50)   NOT NULL
                                 COLLATE NOCASE
                                 DEFAULT 'MD5',
    UserSex      INTEGER         DEFAULT 1,
    UserBirthday DATETIME        COLLATE NOCASE,
    UserEmail    VARCHAR (50)    COLLATE NOCASE,
    UserQq       VARCHAR (15)    COLLATE NOCASE,
    UserWx       VARCHAR (50)    COLLATE NOCASE,
    UserAvatar   VARCHAR (150)   COLLATE NOCASE,
    UserPhone    VARCHAR (11)    COLLATE NOCASE,
    UserGroupId  VARCHAR (40),
    UserStatus   INTEGER         NOT NULL
                                 DEFAULT 1,
    CreateUserId VARCHAR (40),
    CreateTime   DATETIME        NOT NULL
                                 DEFAULT (CURRENT_TIMESTAMP),
    EditTime     DATETIME,
    DeleteSign   INTEGER         NOT NULL
                                 DEFAULT 1,
    DeleteTime   DATETIME,
    Note         NVARCHAR (2048) COLLATE NOCASE,
    PRIMARY KEY (
        UserId
    )
);

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         'admin',
                         '管理员',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         2,
                         '2018-11-21 00:00:00',
                         NULL,
                         NULL,
                         NULL,
                         '/upload/23826636-2891-4f01-95e2-25e3539388a3.jpg',
                         NULL,
                         NULL,
                         1,
                         NULL,
                         '2018-11-20 13:08:03',
                         '2018-11-21 11:52:10.0499535',
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '4b12d20e6e44463e91cefb1031601744',
                         'test1',
                         'test1',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 01:58:37',
                         NULL,
                         2,
                         '2018-11-21 10:39:19',
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'fc545bfd18a54bfcb69b90af49a03336',
                         'test2',
                         'test2',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 01:59:09',
                         NULL,
                         2,
                         '2018-11-21 10:11:16',
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '3ed4de7c2ae046cba574bb0dc818801a',
                         'test3',
                         'test3',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         0,
                         NULL,
                         'ww@163.com',
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:09:00',
                         '2018-11-21 10:38:32.4614563',
                         2,
                         '2018-11-21 10:39:19',
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '41faaf517168485584555989d76176c6',
                         'test4',
                         'test4',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:09:19',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '4a184021a69f40f8a7dc4981832c6484',
                         'test5',
                         'test5',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:09:32',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '306fd401dfa74435a3a43f6b15010f85',
                         'test6',
                         'test6',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:09:46',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '199a8f2da3ee44bb96ec3ba45cbee789',
                         'test7',
                         'test7',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:10:03',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'd76a7cd98b4d40aabf8c39e38a940ce8',
                         'test8',
                         'test8',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:10:18',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         '935f83c23ffc48078ef8ba36178cc483',
                         'test9',
                         'test9',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:10:33',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'f2fdf9206b38421497bbb6331693c93b',
                         'test10',
                         'test10',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:10:47',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );

INSERT INTO Sys_User (
                         UserId,
                         UserName,
                         UserNikeName,
                         UserPwd,
                         UserSex,
                         UserBirthday,
                         UserEmail,
                         UserQq,
                         UserWx,
                         UserAvatar,
                         UserPhone,
                         UserGroupId,
                         UserStatus,
                         CreateUserId,
                         CreateTime,
                         EditTime,
                         DeleteSign,
                         DeleteTime,
                         Note
                     )
                     VALUES (
                         'a0bcca8e-dc22-473c-8ed7-00967b7be528',
                         'test11',
                         'test11',
                         'E10ADC3949BA59ABBE56E057F20F883E',
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         '0cee06b3-cecf-426c-b01d-5a93611fe721',
                         '2018-11-21 02:36:40',
                         NULL,
                         1,
                         NULL,
                         NULL
                     );


-- 表：Sys_UserGroup
CREATE TABLE Sys_UserGroup (
    UserGroupId       VARCHAR (40)    NOT NULL,
    UserGroupName     NVARCHAR (50)   NOT NULL
                                      COLLATE NOCASE,
    ParentUserGroupId VARCHAR (40),
    CreateUserId      VARCHAR (40),
    CreateTime        DATETIME        NOT NULL
                                      DEFAULT (CURRENT_TIMESTAMP),
    EditTime          DATETIME,
    DeleteSign        INTEGER         NOT NULL
                                      DEFAULT 1,
    DeleteTime        DATETIME,
    Note              NVARCHAR (2048) COLLATE NOCASE,
    PRIMARY KEY (
        UserGroupId
    )
);

INSERT INTO Sys_UserGroup (
                              UserGroupId,
                              UserGroupName,
                              ParentUserGroupId,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              'fee85b6d-47f9-436b-b3c9-6a6f1c2e6d64',
                              'TestGroup1',
                              NULL,
                              '0cee06b3-cecf-426c-b01d-5a93611fe721',
                              '2018-11-21 03:18:44',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_UserGroup (
                              UserGroupId,
                              UserGroupName,
                              ParentUserGroupId,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '0dacb4d8-8805-4a5f-8243-49a63b3814fd',
                              'TestGroup2',
                              NULL,
                              '0cee06b3-cecf-426c-b01d-5a93611fe721',
                              '2018-11-21 03:19:34',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );

INSERT INTO Sys_UserGroup (
                              UserGroupId,
                              UserGroupName,
                              ParentUserGroupId,
                              CreateUserId,
                              CreateTime,
                              EditTime,
                              DeleteSign,
                              DeleteTime,
                              Note
                          )
                          VALUES (
                              '19f3242e-82b7-4474-b0a5-9e72c0790613',
                              'TestGroup3',
                              NULL,
                              '0cee06b3-cecf-426c-b01d-5a93611fe721',
                              '2018-11-21 03:20:01',
                              NULL,
                              1,
                              NULL,
                              NULL
                          );


-- 索引：Sys_User_UQ__Sys_User__C9F284564CEEE8C0
CREATE UNIQUE INDEX Sys_User_UQ__Sys_User__C9F284564CEEE8C0 ON Sys_User (
    UserName DESC
);


COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
