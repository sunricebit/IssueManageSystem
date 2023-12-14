﻿var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<IMSContext>(options => new IMSContext());
builder.Services.AddSingleton<IHashService, HashService>();
builder.Services.AddSingleton<IMailService, MailService>();
builder.Services.AddSingleton(new Intermediate());
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IClassService,ClassService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IChkPgAcessService, ChkPgAcessService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<SettingDAO>();
builder.Services.AddScoped<PermissionDAO>();
builder.Services.AddScoped<IMilestoneService, MilestoneService>();
builder.Services.AddSingleton<ErrorHelper>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "SessionCookie";
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Error/404";
        await next();
    }
});

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Landing}/{action=Index}/{id?}");
});
app.Run();

