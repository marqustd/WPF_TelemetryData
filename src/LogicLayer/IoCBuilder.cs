using Autofac;

namespace LogicLayer
{
    public class IoCBuilder
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<JsonDataProcessor>().As<IJsonDataProcessor>();
            builder.RegisterType<FileHelper>().As<IFileHelper>();
            builder.RegisterType<JsonFileReader>().As<IJsonFileReader>();
            builder.RegisterType<JsonParser>().As<IJsonParser>();
            builder.RegisterType<JsonFileWriter>().As<IJsonFileWriter>();

            return builder.Build();
        }
    }
}