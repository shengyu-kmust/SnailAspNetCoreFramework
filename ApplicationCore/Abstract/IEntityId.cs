using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    /// <summary>
    /// 包含主键的entity
    /// </summary>
    /// <typeparam name="T">主键的类型</typeparam>
    public interface IEntityId<T>: IIdField<T>,IBaseEntity
    {
    }

    public interface IDefalutEntityId : IDefaultIdField
    {

    }

    public interface IIdField<T>
    {
        T Id { get; set; }
    }

    public interface IDefaultIdField:IIdField<Guid>
    {
        new Guid Id { get; set; }
    }
}
