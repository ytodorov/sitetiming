using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.GraphQL.Types
{
    public class BaseObjectGraphType<T> : ObjectGraphType<T>
    {
        public BaseObjectGraphType()
        {
            Name = this.GetType().Name; // https://graphql-dotnet.github.io/docs/migrations/migration3#naming
            Type type = typeof(T);
            List<PropertyInfo> properties = type.GetProperties().OrderBy(p => p.Name).ToList();

            //properties = properties
            //    .Where(p => p.CustomAttributes.Count(a => a.AttributeType == typeof(IgnoreInGraphQlAttribute)) == 0)
            //    .ToList();

            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(object))
                {
                    Field(typeof(NonNullGraphType<ObjectGraphType>), prop.Name);
                }
                else if (prop.PropertyType == typeof(long))
                {
                    Field(typeof(NonNullGraphType<LongGraphType>), prop.Name);
                }
                else if (prop.PropertyType == typeof(long?))
                {
                    Field(type: typeof(LongGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(int))
                {
                    Field(typeof(NonNullGraphType<IntGraphType>), prop.Name);
                }
                else if (prop.PropertyType == typeof(int?))
                {
                    Field(type: typeof(IntGraphType), name: prop.Name);
                }

                if (prop.PropertyType == typeof(byte))
                {
                    Field(typeof(NonNullGraphType<ByteGraphType>), prop.Name);
                }
                else if (prop.PropertyType == typeof(byte?))
                {
                    Field(type: typeof(ByteGraphType), name: prop.Name);
                }

                if (prop.PropertyType == typeof(short))
                {
                    Field(typeof(NonNullGraphType<ShortGraphType>), prop.Name);
                }
                else if (prop.PropertyType == typeof(short?))
                {
                    Field(type: typeof(ShortGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(DateTime?))
                {
                    Field(type: typeof(DateTimeGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    Field(type: typeof(NonNullGraphType<DateTimeGraphType>), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(string))
                {
                    Field(type: typeof(StringGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(decimal?))
                {
                    Field(type: typeof(DecimalGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(decimal))
                {
                    Field(type: typeof(NonNullGraphType<DecimalGraphType>), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(double?))
                {
                    Field(type: typeof(FloatGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(double))
                {
                    Field(type: typeof(NonNullGraphType<FloatGraphType>), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(bool?))
                {
                    Field(type: typeof(BooleanGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    Field(type: typeof(NonNullGraphType<BooleanGraphType>), name: prop.Name);
                }

                // Guid is fixed in 3.0.0-preview-1141 of GraphQL
                else if (prop.PropertyType == typeof(Guid?))
                {
                    Field(type: typeof(GuidGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(Guid))
                {
                    Field(type: typeof(NonNullGraphType<GuidGraphType>), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(DateTimeOffset?))
                {
                    Field(type: typeof(DateTimeOffsetGraphType), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(DateTimeOffset))
                {
                    Field(type: typeof(NonNullGraphType<DateTimeOffsetGraphType>), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(short))
                {
                    Field(type: typeof(NonNullGraphType<ShortGraphType>), name: prop.Name);
                }
                else if (prop.PropertyType == typeof(short?))
                {
                    Field(type: typeof(ShortGraphType), name: prop.Name);
                }
            }
        }
    }
}
