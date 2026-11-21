using Microsoft.EntityFrameworkCore;
using NgoMinhChinh_2026.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (value, name) => $"{name} phải là số hợp lệ.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
        name => $"{name} phải là số.");
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        name => $"{name} không được để trống.");
});
builder.Services.AddDbContext<QLHangHoaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLHangHoa")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
