﻿// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
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
    /// <param name="propertyExpression"></param>
    /// <param name="attribute"></param>
    public void AddPropertyConverter<TEntity>(Expression<Func<TEntity, object?>> propertyExpression, DataPropertyConverterAttribute attribute)
    {
        if (propertyExpression.Body is MemberExpression memberExpression)
        {
            _propertyConverters.AddOrUpdate(memberExpression.Member, m => attribute, (m, v) => attribute);
        }
    }

    /// <summary>
    /// 获得指定数据类型属性转换器方法
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public bool TryGetPropertyConverter<TEntity>(Expression<Func<TEntity, object?>> propertyExpression, [NotNullWhen(true)] out DataPropertyConverterAttribute? converterAttribute)
    {
        converterAttribute = null;
        var ret = false;
        if (propertyExpression.Body is MemberExpression memberExpression && TryGetPropertyConverter<TEntity>(memberExpression.Member, out var v))
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
