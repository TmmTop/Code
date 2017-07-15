using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("LwmReportView")]
[assembly: AssemblyDescription("桌面应用程序的自定义定制报表组件;     使用说明：VS包管理工具直接安装包 Install - Package LwmReportView@1.0.0.3(此版本为稳定版);      先将nuget安装的包的package目录里的content/Libs里的dll引用进项目;      点击工具箱，右键选项 将nuget安装的包 LwmReportView.dll 添加进去;     拖到窗体即可添加控件;     页面加载执行 view1.LoadData('测试', '20170713 0919', '安式软件', string.Format(@'总金额:{0}', '999999'), DoddleProductRepository.GetAll());     参数是标题 ，创建时间，页脚，头部标题（可以显示统计分析的数据，可自定义）;      对象集合可以参考TestModel.cs")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("李为明")]
[assembly: AssemblyProduct("报表定制组件")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("8072efc4-6708-407a-b585-fc6952b73ab2")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
// 可以指定所有值，也可以使用以下所示的 "*" 预置版本号和修订号
// 方法是按如下所示使用“*”: :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.3")]
[assembly: AssemblyFileVersion("1.0.0.3")]
