﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "MN")]
#endif
    #endregion
    public abstract class MemberNode<TMemberInfo> : Node where TMemberInfo : MemberInfo
    {
        protected MemberNode() { }

        protected MemberNode(INodeFactory factory, TMemberInfo memberInfo)
            : base(factory)
        {
            if (memberInfo != null)
                this.Initialize(memberInfo);
        }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "D")]
#endif
        #endregion
        public TypeNode DeclaringType { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "M")]
#endif
        #endregion
        public MemberTypes MemberType { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "S")]
#endif
        #endregion
        public string Signature { get; set; }

        protected virtual void Initialize(TMemberInfo memberInfo)
        {
            this.DeclaringType = this.Factory.Create(memberInfo.DeclaringType);
            this.MemberType = memberInfo.MemberType;
            this.Signature = memberInfo.ToString();
        }

        protected Type GetDeclaringType(ExpressionContext context)
        {
            if (this.DeclaringType == null)
                throw new InvalidOperationException("DeclaringType is not set.");

            var declaringType = this.DeclaringType.ToType(context);
            if (declaringType == null)
                throw new TypeLoadException("Failed to load DeclaringType: " + this.DeclaringType);

            return declaringType;
        }

        protected abstract IEnumerable<TMemberInfo> GetMemberInfosForType(Type type);

        public virtual TMemberInfo ToMemberInfo(ExpressionContext context)
        {
            if (string.IsNullOrWhiteSpace(this.Signature))
                return null;

            return this.GetMemberInfosForType(this.GetDeclaringType(context)).First(m => m.ToString() == this.Signature);
        }
    }
}