# SnBlogCore

基于 **ASP.NET Core 8** 的博客管理系统后端 API，采用 Entity Framework Core + MySQL 数据持久化、JWT 认证、Swagger/Knife4jUI 接口文档。

---

## 技术栈

| 层次 | 技术 | 版本 |
|------|------|------|
| 运行时 | .NET 8.0 | 8.0 |
| ORM | Entity Framework Core + Pomelo MySQL | 8.0.1 / 8.0.0-beta.2 |
| 数据库 | MySQL | 8.0.33 |
| 认证 | JWT Bearer (HMAC-SHA256) | 8.0.1 |
| 对象映射 | AutoMapper | 12.0.1 |
| 验证 | FluentValidation | 11.3.0 |
| API 文档 | Swashbuckle + Knife4jUI | 6.5.0 / 0.0.16 |
| 限流 | ASP.NET Core Rate Limiter (Token Bucket) | 内置 |
| 全球化 | InvariantGlobalization | — |

---

## 项目结构

```
SnBlogCore/
├── SnBlogCore.sln                         # 解决方案文件
├── CommonLibrary/                         # 公共类库
│   └── DI 模块初始化支持（IModuleInit）
└── SnBlogCore/                            # 主 Web API 项目
    ├── Program.cs                         # 应用入口（最小托管模型）
    ├── appsettings.json                   # 主配置（JWT / CORS / Hosts）
    ├── appsettings.Development.json       # 开发环境配置（含数据库连接串）
    ├── appsettings.Production.json        # 生产环境配置
    ├── AutoMapper/
    │   ├── DTO/ArticleDto.cs              # 文章数据传输对象
    │   └── Mapper/ArticleMapper.cs        # AutoMapper 映射配置
    ├── Controllers/
    │   ├── Articles/                      # 文章模块
    │   │   ├── ArticleController.cs       # 文章 API 控制器
    │   │   ├── ArticleService.cs          # 文章业务逻辑
    │   │   └── IArticleService.cs         # 文章服务接口
    │   ├── Users/UserController.cs        # 用户登录控制器
    │   ├── Test/TestController.cs         # 测试/调试控制器
    │   └── Uploading/UploadController.cs  # 上传控制器（待实现）
    ├── Jwt/JwtHelper.cs                   # JWT Token 生成帮助类
    ├── models/                            # 数据实体模型（EF Core Database-First）
    │   ├── SnblogContext.cs               # DbContext 数据库上下文
    │   ├── Article.cs / ArticleTag.cs / ArticleType.cs
    │   ├── Diary.cs / DiaryType.cs
    │   ├── Interface.cs / InterfaceType.cs
    │   ├── Navigation.cs / NavigationType.cs
    │   ├── Photo.cs / PhotoGallery.cs / PhotoGalleryType.cs / PhotoType.cs
    │   ├── SnComment.cs
    │   ├── SnPicture.cs / SnPictureType.cs
    │   ├── SnSetblog.cs / SnSetblogType.cs
    │   ├── SnSoftware.cs / SnSoftwareType.cs
    │   ├── SnTalk.cs / SnTalkType.cs
    │   ├── SnUserFriend.cs / SnVideoType.cs
    │   ├── Snippet.cs / SnippetTag.cs / SnippetType.cs / SnippetTypeSub.cs / SnippetVersion.cs
    │   ├── User.cs / UserTalk.cs
    │   └── Video.cs
    ├── Swagger/                           # Swagger 分组配置
    │   ├── ApiGroupAttribute.cs
    │   ├── ApiGroupNames.cs               # 枚举：Login / v1
    │   └── GroupInfoAttribute.cs
    └── Validator/ArticleValidator.cs      # FluentValidation 文章验证规则
```

---

## API 端点

### Swagger 文档

启动项目后访问根路径 `/index.html` 即可查看 Knife4jUI 接口文档。

API 分为三个分组：

| 分组 | 说明 |
|------|------|
| **登录认证** | JWT 登录相关接口 |
| **v1** | v1 版本业务接口 |
| **无分组** | 未分类接口 |

### 端点一览

| 方法 | 路由 | 认证 | 说明 |
|------|------|------|------|
| `GET` | `/article/sum` | 无 | 查询文章总数（10 秒响应缓存） |
| `GET` | `/article/all?cache=` | JWT | 查询所有文章（需认证） |
| `GET` | `/user/login?user=&pwd=` | 无 | 用户登录，返回 JWT Token |
| `POST` | `/test` | 无 | 读取配置中的 `test` 值 |
| `POST` | `/test/TestModulInit` | 无 | 测试 CommonLibrary 模块初始化 |
| `GET` | `/WeatherForecast` | 无 | 模板示例（无实际业务） |

---

## 数据模型

项目包含 **30 个实体模型**，均为 `partial class`，通过 EF Core Scaffold 从 MySQL 数据库反向工程生成。

核心业务模块：

| 模块 | 相关模型 | 说明 |
|------|----------|------|
| 博客文章 | `Article`, `ArticleTag`, `ArticleType` | 博客文章、标签、分类 |
| 代码片段 | `Snippet`, `SnippetTag`, `SnippetType`, `SnippetTypeSub`, `SnippetVersion` | 代码片段管理含版本控制 |
| 图片/图册 | `Photo`, `PhotoGallery`, `PhotoGalleryType`, `PhotoType`, `SnPicture`, `SnPictureType` | 图片与图床管理 |
| 日记 | `Diary`, `DiaryType` | 日记功能 |
| 说说/话题 | `SnTalk`, `SnTalkType`, `UserTalk` | 用户说说与话题 |
| 导航 | `Navigation`, `NavigationType` | 导航菜单 |
| 接口管理 | `Interface`, `InterfaceType` | API 接口元数据管理 |
| 软件资源 | `SnSoftware`, `SnSoftwareType` | 软件资源分享 |
| 评论 | `SnComment` | 站内评论 |
| 用户 | `User`, `SnUserFriend` | 用户与好友关系 |
| 视频 | `Video`, `SnVideoType` | 视频内容 |
| 博客设置 | `SnSetblog`, `SnSetblogType` | 博客全局设置 |

**DbContext**: `SnblogContext`，使用 `utf8mb3` 字符集与 `utf8mb3_general_ci` 排序规则。

---

## 快速开始

### 环境要求

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MySQL 8.0+

### 1. 克隆仓库

```bash
git clone <repository-url>
cd SnBlogCore
```

### 2. 配置数据库

编辑 `SnBlogCore/appsettings.Development.json`，修改数据库连接字符串：

```json
"ConnectionStrings": {
  "MysqlConnection": "Server=localhost;database=snblog;uid=root;pwd=你的密码"
}
```

### 3. 初始化数据库

项目采用 **Database-First** 方式，模型已由数据库反向生成。请确保 MySQL 中已存在 `snblog` 数据库及对应的表结构。

如需迁移，使用 EF Core 工具：

```bash
cd SnBlogCore
dotnet ef database update
```

### 4. 运行项目

```bash
dotnet run --project SnBlogCore
```

启动后访问：

| 地址 | 说明 |
|------|------|
| `http://localhost:5186` | HTTP 端点 |
| `https://localhost:7012` | HTTPS 端点 |
| `http://localhost:5186/index.html` | Knife4jUI 接口文档 |

### 5. 登录获取 Token

```bash
curl "http://localhost:5186/user/login?user=你的用户名&pwd=你的密码"
```

返回的 `Token` 在 Swagger 页面右上角点击 **Authorize** 按钮，填入 `Bearer {token}` 即可测试需要认证的接口。

---

## 配置说明

### JWT 配置（appsettings.json）

```json
{
  "Jwt": {
    "SecretKey": "你的密钥（至少 256bit）",
    "Issuer": "WebAppIssuer",
    "Audience": "WebAppAudience",
    "Expiration": 4800  // 过期时间（分钟）
  }
}
```

### CORS 配置

```json
{
  "CorsUrls": "http://localhost:4001,https://你的前端域名"
}
```

### 限流策略

项目使用 **Token Bucket（令牌桶）** 算法进行限流：

| 参数 | 值 | 说明 |
|------|-----|------|
| `TokenLimit` | 10 | 令牌桶容量 |
| `TokensPerPeriod` | 2 | 每次放入令牌数 |
| `ReplenishmentPeriod` | 5s | 放令牌间隔 |

> 限流中间件已启用但暂未绑定到具体端点，需要时取消 `[EnableRateLimiting("TokenLimit")]` 注解即可。

---

## 中间件管道

```
UseRateLimiter → UseHttpsRedirection → UseCors → UseAuthentication → UseAuthorization → MapControllers
```

---

## 发布部署

```bash
dotnet publish -c Release -r win-x64 --no-self-contained -o ./publish
```

---

## 安全建议

> ⚠️ 以下为已知安全风险，建议在正式环境使用前处理：

1. **密码明文存储** — 数据库中用户密码直接存储明文，`UserController.Login` 使用 `u.Pwd == pwd` 比较。建议使用 BCrypt / PBKDF2 进行哈希存储。
2. **密钥硬编码** — `appsettings.json` 中硬编码了 JWT SecretKey，建议迁移至 [User Secrets](https://learn.microsoft.com/zh-cn/aspnet/core/security/app-secrets) 或环境变量。
3. **Pomelo.EntityFrameworkCore.MySql 使用 beta 版** — 当前版本为 `8.0.0-beta.2`，生产环境建议使用稳定版。

---

## 待改进项

| 优先级 | 项目 | 说明 |
|--------|------|------|
| 🔴 高 | 密码加密 | 使用 BCrypt/PBKDF2 替代明文存储 |
| 🔴 高 | 密钥管理 | JWT SecretKey 迁移至 User Secrets / 环境变量 |
| 🟡 中 | 架构整理 | 服务层移出 Controllers 目录，统一命名空间 |
| 🟡 中 | CRUD 完善 | 为各实体补充完整的增删改查端点 |
| 🟡 中 | `UploadController` | 实现文件上传功能，修复路由拼写（`uoload` → `upload`） |
| 🟡 中 | 依赖注入规范 | `ArticleController` 直接注入 `IArticleService` 而非 `IServiceProvider` |
| 🟢 低 | 单元测试 | 引入 xUnit/NUnit 覆盖核心业务逻辑 |
| 🟢 低 | Docker 支持 | 添加 Dockerfile 便于容器化部署 |
| 🟢 低 | 日志增强 | 引入 Serilog 结构化日志 |

---

## 许可证

MIT
