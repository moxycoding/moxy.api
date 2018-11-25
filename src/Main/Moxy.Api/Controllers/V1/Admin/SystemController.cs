﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moxy.Core;
using Moxy.Framework.Authentication;
using Moxy.Framework.Filters;
using Moxy.Framework.Permissions;
using Moxy.Services.System;
using Moxy.Services.System.Dtos;

namespace Moxy.Api.Controllers.V1.Admin
{
    /// <summary>
    /// 系统管理接口
    /// </summary>
    [MoxyModule(Order = 100, ModuleName = "系统管理")]
    public class SystemController : BaseAdminController
    {
        /// <summary>
        /// SystemController
        /// </summary>
        private readonly ISystemService _systemService;
        private readonly IMoxyAuth _moxyAuth;
        private readonly IWebContext _webContext;
        public SystemController(ISystemService systemService
            , IMoxyAuth moxyAuth
            , IWebContext webContext
            )
        {
            _systemService = systemService;
            _moxyAuth = moxyAuth;
            _webContext = webContext;
        }
        #region 管理员管理
        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("admin/list")]
        [Permission("system_admin_list", "管理员列表", true)]
        public IActionResult AdminList(SysAdminSearchRequest request)
        {
            var result = _systemService.GetAdminList(request);
            return Ok(OperateResult.Succeed("ok", result));
        }
        /// <summary>
        /// 管理员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("admin/item")]
        [RelyPermission("system_admin_create", "system_admin_edit")]
        public IActionResult AdminItem(int id)
        {
            var result = _systemService.GetAdminItem(id);
            return Ok(OperateResult.Succeed("ok", result));
        }
        /// <summary>
        /// 管理员创建
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("admin/create")]
        [Permission("system_admin_create", "管理员创建")]
        [ModelValid]
        public IActionResult AdminCreate([FromBody]SysAdminInputDto input)
        {
            var result = _systemService.CreateAdmin(input);
            return Ok(result);
        }
        /// <summary>
        /// 管理员编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("admin/edit")]
        [Permission("system_admin_edit", "管理员编辑")]
        [ModelValid("AdminPwd")]
        public IActionResult AdminEdit([FromBody]SysAdminInputDto input)
        {
            var result = _systemService.UpdateAdmin(input);
            return Ok(result);
        }
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("admin/delete")]
        [Permission("system_admin_delete", "管理员删除")]
        public IActionResult DeleteAdmin([FromBody]IdsRequest<int> request)
        {
            var result = _systemService.DeleteAdmin(request.Ids);
            return Ok(result);
        }
        /// <summary>
        /// 系统模块
        /// </summary>
        /// <returns></returns>
        [RelyPermission("system_admin_create", "system_admin_edit")]
        [Route("admin/modules")]
        [HttpGet]
        public IActionResult AdminModules()
        {
            Assembly assembly = Assembly.Load(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            Dictionary<string, List<dynamic>> dics = new Dictionary<string, List<dynamic>>();
            var types = assembly.GetTypes()
                                .AsEnumerable()
                                .Where(type => typeof(BaseAdminController).IsAssignableFrom(type))
                                .OrderBy(type => type.GetCustomAttribute<MoxyModuleAttribute>()?.Order)
                                .ToList();
            foreach (var type in types)
            {
                var members = type.GetMethods();
                var moduleList = new List<dynamic>();
                foreach (var member in members)
                {
                    if (!typeof(IActionResult).IsAssignableFrom(member.ReturnType))
                        continue;
                    var moduleAttr = member.GetCustomAttribute<PermissionAttribute>();
                    if (moduleAttr == null)
                        continue;
                    moduleList.Add(new
                    {
                        moduleAttr.ModuleName,
                        moduleAttr.ModuleCode,
                        moduleAttr.IsPage

                    });
                }
                if (moduleList.Count == 0)
                    continue;
                var moduleName = type.GetCustomAttribute<MoxyModuleAttribute>()?.ModuleName ?? "默认";
                if (dics.ContainsKey(moduleName))
                {
                    dics[moduleName].AddRange(moduleList);
                }
                else
                {
                    dics.Add(moduleName, moduleList);
                }
            }
            return Ok(OperateResult.Succeed("ok", dics));
        }
        #endregion


        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("admin/updatepwd")]
        [Permission("system_admin_updatepwd", "管理员密码修改")]
        [ModelValid]
        public IActionResult UpdatePwd([FromBody]SysAdminUpdatePwdInputDto input)
        {
            input.AdminName = _webContext.AuthName;
            var result = _systemService.UpdatePwd(input);
            return Ok(result);
        }
    }
}