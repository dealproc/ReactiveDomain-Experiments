using System;
using System.Collections.Generic;
using System.Linq;

namespace RD.Core.ValueObjects {
    public class Scopes : List<Scope> {
        public override string ToString() => string.Join("|", this.Select(s => s.ToString()));
        public static Scopes FromString(string value) {
            var result = new Scopes();
            var values = value.Split(new [] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new Scope(x));
            result.AddRange(values);
            return result;
        }
    }
}