﻿using AppShareServices.Pagging;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerApplication.Services;

namespace WorkerCrossCutting.DependencyInjections
{
    /// <summary>
    /// Mapping, Pagging
    /// </summary>
    public class ApplicationInjector
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUriService>(o =>
            {
                var request = o.GetRequiredService<IHttpContextAccessor>().HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                //TODO: https://codewithmukesh.com/blog/pagination-in-aspnet-core-webapi/
                return new UriService(uri);
            });

            services.AddScoped<IWorkerManager, WorkerManager>();
        }
    }
}