// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Longbow.Sockets.DataConverters;

/// <summary>
/// 数据转换器集合类
/// </summary>
public sealed class DataConverterCollection
{
    readonly ConcurrentDictionary<MemberInfo, DataPropertyConverterAttribute> _propertyConverters = new();

    /// <summary>
    /// 添加属性类型转化器方法
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public void AddPropertyConverter<TEntity>(Expression<Func<TEntity, object?>> memberExpression, DataPropertyConverterAttribute attribute)
    {
        if (memberExpression.Body is MemberExpression memberInfoExpression)
        {
            _propertyConverters.AddOrUpdate(memberInfoExpression.Member, m => attribute, (m, v) => attribute);
        }
    }

    /// <summary>
    /// 获得指定数据类型属性转换器方法
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public bool TryGetPropertyConverter<TEntity>(Expression<Func<TEntity, object?>> memberExpression, [NotNullWhen(true)] out DataPropertyConverterAttribute? converterAttribute)
    {
        converterAttribute = null;
        var ret = false;
        if (memberExpression.Body is MemberExpression memberInfoExpression && TryGetPropertyConverter<TEntity>(memberInfoExpression.Member, out var v))
        {
            converterAttribute = v;
            ret = true;
        }
        return ret;
    }

    /// <summary>
    /// 获得指定数据类型属性转换器方法
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public bool TryGetPropertyConverter<TEntity>(MemberInfo memberInfo, [NotNullWhen(true)] out DataPropertyConverterAttribute? converterAttribute)
    {
        converterAttribute = null;
        var ret = false;
        if (_propertyConverters.TryGetValue(memberInfo, out var v))
        {
            converterAttribute = v;
            ret = true;
        }
        return ret;
    }
}
