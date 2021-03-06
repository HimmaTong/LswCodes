﻿using Asp.Net_Indetity.EntityFramework;
using Asp.Net_Indetity.Models;
using Asp.Net_Indetity.Validator;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp.Net_Indetity.Manager
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
        : base(store)
        {
        }



        public static AppUserManager Create(
                IdentityFactoryOptions<AppUserManager> options,
                IOwinContext context)
        {

            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();
            //UserStore<T> 是 包含在 Microsoft.AspNet.Identity.EntityFramework 中，它实现了 UserManger 类中与用户操作相关的方法。
            //也就是说UserStore<T>类中的方法（诸如：FindById、FindByNameAsync...）通过EntityFramework检索和持久化UserInfo到数据库中
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            //自定义的Password Validator
            manager.PasswordValidator = new CustomPasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true
            };
            //自定义的Password Validator
            manager.UserValidator = new CustomUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            return manager;
        }


    }
}