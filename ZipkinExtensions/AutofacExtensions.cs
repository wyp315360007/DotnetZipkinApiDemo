#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2020 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  AutofacExtensions 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng
     * 创建时间： 2020/10/14 14:42:23 
     * 描述    :
     * =====================================================================
     * 修改时间：2020/10/14 14:42:23 
     * 修改人  ： wangyunpeng
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion


using Autofac;

namespace ZipkinExtensions
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder AddZipkin(this ContainerBuilder containerBuilder, string serviceName)
        {
            containerBuilder.RegisterType<TraceObserver>()
                            .SingleInstance()
                            ;
            containerBuilder.RegisterType<HttpDiagnosticListener>()
                            .WithParameter(
                                   (pi, c) => pi.ParameterType == typeof(string)
                                 , (pi, c) => serviceName   //指定服务名称
                            )
                            .AsImplementedInterfaces()
                            .SingleInstance()
                            ;

            return containerBuilder;
        }
    }
}
