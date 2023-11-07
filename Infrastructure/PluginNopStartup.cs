using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Factories;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Validators;
using Nop.Plugin.Misc.SimpleLMS.Services;

namespace Nop.Plugin.Misc.SimpleLMS.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            services.AddTransient<IValidator<CourseModel>, CourseValidator>();
            services.AddTransient<IValidator<AttachmentModel>, AttachmentValidator>();
            services.AddTransient<IValidator<LessonModel>, LessonValidator>();
            services.AddTransient<IValidator<SectionModel>, SectionValidator>();
            services.AddTransient<IValidator<VideoModel>, VideoValidator>();

            
            services.AddScoped<CourseService>();
            services.AddScoped<CourseModelFactory>();
            services.AddScoped<AdminCourseModelFactory>();

            //services.AddScoped<CustomModelFactory, ICustomerModelFactory>();


        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => 10000;
    }
}