# 🔥 Hot Reload Configuration Guide - Clinic.Web

## Overview
Hot Reload cho phép ứng dụng tự động refresh khi bạn sửa code mà không cần restart lại server.

## ✅ Configuration Completed

### 1. **Program.cs Updated**
   - Thêm `AddDeveloperExceptionPage()` trong Development environment
   - Giúp hot reload hoạt động tốt hơn

### 2. **launchSettings.json Updated**
   - Thêm profile `watch` mới
   - Cấu hình auto-refresh cho development

---

## 🚀 How to Use (3 Ways)

### **Way 1: Visual Studio Built-in Hot Reload** (Recommended)
1. Mở project `Clinic.Web` trong Visual Studio
2. Nhấn `F5` để start debugging (hoặc Ctrl+F5 không debug)
3. Sửa code bất kỳ file nào (`.cshtml`, `.cs`, `.css`)
4. Nhấn `Alt+F10` để trigger hot reload (hoặc tự động khi lưu file)
5. Web sẽ refresh tự động

**Tính năng hỗ trợ:**
- ✅ Razor Pages markup (`.cshtml`)
- ✅ C# code-behind (`.cshtml.cs`)
- ✅ CSS files
- ✅ JavaScript (một số trường hợp)

---

### **Way 2: dotnet watch (Terminal)**
1. Mở Terminal/PowerShell tại folder `Clinic.Web`
2. Chạy lệnh:
   ```bash
   dotnet watch
   ```
3. Hoặc chạy với profile cụ thể:
   ```bash
   dotnet watch --launch https
   ```
4. Sửa code → tự động rebuild và refresh

**Ưu điểm:**
- Nhanh hơn full rebuild
- Dễ theo dõi build messages
- Tốt cho CI/CD workflows

---

### **Way 3: Visual Studio Launch Configuration**
1. Nhấn vào dropdown bên cạnh nút `Start` (▶)
2. Chọn "Edit Configurations..."
3. Chọn profile `watch` (vừa tạo)
4. Nhấn `F5` để start

---

## ⚙️ Configuration Details

### Program.cs Changes:
```csharp
// Enable hot reload in development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDeveloperExceptionPage();
}
```

### launchSettings.json Changes:
```json
"watch": {
  "commandName": "Executable",
  "executablePath": "dotnet",
  "commandLineArgs": "watch --project Clinic.Web.csproj",
  "workingDirectory": ".",
  "launchBrowser": true,
  "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
  }
}
```

---

## 🔍 Troubleshooting

### Problem: Hot reload không hoạt động
**Solution:**
1. Đảm bảo đang ở Development environment
2. Kiểm tra `ASPNETCORE_ENVIRONMENT` = "Development"
3. Restart Visual Studio
4. Clear cache: `dotnet build --clean`

### Problem: Chỉ một số file không auto-refresh
**Solution:**
- Một số thay đổi (config, dependencies) cần full rebuild
- Nhấn `Ctrl+Shift+B` để force rebuild
- Hoặc nhấn `F5` để restart app

### Problem: CSS/JavaScript không cập nhật
**Solution:**
1. Clear browser cache (Ctrl+Shift+Delete)
2. Hard refresh: `Ctrl+F5` hoặc `Ctrl+Shift+R`
3. Kiểm tra `wwwroot` folder structure

---

## 📝 Best Practices

1. **Lưu file thường xuyên** (Ctrl+S)
   - Hot reload khích hoạt khi file được lưu

2. **Sử dụng Browser DevTools**
   - Mở DevTools (F12) để debug real-time
   - Network tab để xem requests

3. **Kiểm tra Console Output**
   - Visual Studio Output window hiển thị build status
   - Terminal window (nếu dùng `dotnet watch`)

4. **Một số thay đổi cần full restart:**
   - Connection strings
   - Dependency Injection setup
   - Entity Framework migrations
   - `appsettings.json` changes

---

## 🎯 Quick Reference

| Action | Shortcut |
|--------|----------|
| Start with Debug | `F5` |
| Start without Debug | `Ctrl+F5` |
| Trigger Hot Reload | `Alt+F10` |
| Force Rebuild | `Ctrl+Shift+B` |
| Hard Refresh Browser | `Ctrl+F5` |
| Clear Cache | `Ctrl+Shift+Delete` |

---

## 📚 Additional Resources

- [Microsoft Hot Reload Documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-watch)
- [ASP.NET Core Hot Reload Guide](https://learn.microsoft.com/en-us/aspnet/core/hot-reload)
- [Razor Pages with Hot Reload](https://learn.microsoft.com/en-us/aspnet/core/razor-pages)

---

**Mọi thứ đã được cấu hình sẵn! 🎉**
Bây giờ chỉ cần sửa code và nhìn app refresh tự động.
