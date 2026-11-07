![Banner](docs/banner.png)

# SketchNow

[![Issues](https://img.shields.io/github/issues/realybin/SketchNow.svg)](https://github.com/realybin/SketchNow/issues)

An application brings the ability to draw on your screen(anywhere)

> [!CAUTION]
> Still developing. To avoid potential risks, please don't install in the production environment

### Feature

- Realtime drawing on your screen
- High performance and welly designed UI/UX
- Stable and reliable
- Never share your data
- Supports pressure sensitive devices
- Low cost of system resources

### Upcoming

See projects [here](https://github.com/realybin/SketchNow/projects)

### Install

Go [release](https://github.com/realybin/SketchNow/releases), and choose the version you want.

If you want to have a taste of the latest commited code,
you can go [actions](https://github.com/realybin/SketchNow/actions),
and download the `Portable` or `Installer` artifact

### Privacy
| Permission    | Detail                                                                                                |
|---------------|-------------------------------------------------------------------------------------------------------|
| Data Storage  | Installer version `%LocalAppData%\SketchNow` <br/> Portable version Where the main program is located |
| Network Usage | **GET** `github.com` the version information to download and upgrade your application                 |

### Contribute
1. Follow the `.editorconfig`
2. Use `Xaml Styler` to format xaml (Xaml Styler file at [Settings.XamlStyler](Settings.XamlStyler))
3. Follow [`Conventional Commits`](https://www.conventionalcommits.org/en/v1.0.0/)
4. Follow [`Semantic Versioning`](https://semver.org/)
### Build
- Windows 10 or greater
- Visual Studio 2026/JetBrains Rider
- .NET Desktop workload with `.NET 10`
### Publish
We use [Velopack](https://github.com/velopack/velopack) to generate releases.

```shell
dotnet tool restore
dotnet publish SketchNow/SketchNow.csproj -c Release --self-contained true -r win-x64 -o .\publish
dotnet vpk pack --packId SketchNow --packVersion 1.1.0 --packDir .\publish --mainExe SketchNow.exe --runtime win-x64 --channel win --outputDir .\Releases
```
Use the application version from `SketchNow/SketchNow.csproj` for `--packVersion`.
### Test
- xUnit v3
- Moq
### Credit
- [DotnetTemplates](https://github.com/Keboo/DotnetTemplates)
- [wpf](https://github.com/dotnet/wpf)
- [CommunityToolkit MVVM](https://github.com/CommunityToolkit/dotnet)
- [Material Design in XAML](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [ValueConverters.NET](https://github.com/thomasgalliker/ValueConverters.NET)
- [WPF.JoshSmith.Controls.DragCanvas](https://github.com/denxorz/WPF.JoshSmith.Controls.DragCanvas)
- [Velopack](https://github.com/velopack/velopack)
- Moq
- System.Text.Json
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.DependencyInjection

![Screenshot of you are using a desktop application with SketchNow](docs/screenshot.png)
