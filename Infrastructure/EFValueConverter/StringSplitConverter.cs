using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.EFValueConverter
{
    public class StringSplitConverter : ValueConverter<List<string>, string>
    {
        public StringSplitConverter(Expression<Func<List<string>, string>> convertToProviderExpression, Expression<Func<string, List<string>>> convertFromProviderExpression, ConverterMappingHints mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }

        public static StringSplitConverter DefaultConverter => new StringSplitConverter(a => a == null ? "" : string.Join(",", a), a => string.IsNullOrEmpty(a) ? new List<string>() : a.Split(new char[] { ',' }).ToList());

    }
}
