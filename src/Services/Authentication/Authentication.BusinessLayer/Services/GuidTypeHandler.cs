using Dapper;
using System.Data;

namespace Authentication.BusinessLayer.Services
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override Guid Parse(object value)
        {
            if (value is Guid)
                return (Guid)value;
            return Guid.Parse((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, Guid value) =>
            parameter.Value = value.ToString();
    }
}
