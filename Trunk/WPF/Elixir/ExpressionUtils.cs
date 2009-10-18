using System;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using System.Linq;

namespace Elixir
{
    public static class ExpressionUtils
    {
        // Cannot do this because it blows up with a field access exception
        //public static object GetExpressionRootObject(Expression expression)
        //{
        //    var lambda = expression as LambdaExpression;
        //    MemberExpression member;

        //    if (lambda != null)
        //        member = (MemberExpression)lambda.Body;
        //    else
        //        member = (MemberExpression)expression;

        //    while (member != null && !(member.Expression is ConstantExpression))
        //    {
        //        member = member.Expression as MemberExpression;
        //    }

        //    if (member != null)
        //    {
        //        ConstantExpression constant = member.Expression as ConstantExpression;
        //        if (constant != null)
        //        {
        //            var field = member.Member as FieldInfo;

        //            if (field != null)
        //            {
        //                return field.GetValue(constant.Value);
        //            }
        //        }
        //    }

        //    return null;
        //}

        //Hack: We need to clean this up to support deeper property trees
        public static INotifyPropertyChanged GetModelFromExpression(LambdaExpression vmExpression)
        {
            var body = vmExpression.Body;
            MemberInfo memberInfo = null;
            ConstantExpression constantAccess = null;
            MemberExpression memberAccess = null;
            MemberExpression memberAccess2 = null;


            if (body is MemberExpression)
            {
                memberAccess = body as MemberExpression;

                while (memberAccess.Expression is MemberExpression)
                {
                    memberAccess = memberAccess.Expression as MemberExpression;
                }
            }
            else
            {
                MethodCallExpression methodEx = (MethodCallExpression)body;

                memberAccess = (MemberExpression)methodEx.Object;

            }

            constantAccess = memberAccess.Expression as ConstantExpression;

            // this piece is NOT generic... Here's where you need to do something to handle
            //   whether this is a "field" or a "property".
            memberInfo = ((MemberInfo)memberAccess.Member);

            // we are pruning back to the last "dot" in the expression that was passed in,
            //   and removing the last "suffix" from this.
            ConstantExpression constantAccess2 = Expression.Constant(constantAccess.Value);

            if (memberInfo is FieldInfo)
            {
                memberAccess2 = Expression.Field(constantAccess2, memberInfo as FieldInfo);
            }
            else
            {
                memberAccess2 = Expression.Property(constantAccess2, memberInfo as PropertyInfo);
            }

            var lambda = Expression.Lambda(memberAccess2);
            var lambdaFunc = lambda.Compile();
            object vm = lambdaFunc.DynamicInvoke();

            return vm as INotifyPropertyChanged;
        }

        public static string GetMethodFromExpression(Expression method)
        {
            var lambda = method as LambdaExpression;
            MethodCallExpression methodExpression = lambda.Body as MethodCallExpression;
            return methodExpression.Method.Name;
        }

        public static string GetExpressionPropertyPath(Expression property)
        {
            var lambda = property as LambdaExpression;
            MemberExpression member;

            if (lambda != null)
                member = (MemberExpression)lambda.Body;
            else
                member = (MemberExpression)property;

            // Expression of (parent.child.value) results in 3 MemberExpressions: value, child, parent
            // parent.Expression is a ConstantExpression that represents the closure
            // We want child.value, so we check the parent member to see if its Expression
            // is a Constant.  We skip the expression whose expression is a constant.
            string path = null;
            var parentMember = member.Expression as MemberExpression;
            if (parentMember != null && !(parentMember.Expression is ConstantExpression))
            {
                path = GetExpressionPropertyPath(member.Expression) + ".";
            }

            return path + member.Member.Name;
        }

        public static bool ExpressionEquals<T>(this Expression<Action<T>> left, Expression<Action<T>> right)
        {
            string leftAction = GetMethodFromExpression(left);
            string rightAction = GetMethodFromExpression(right);
            return (leftAction.Equals(rightAction));
        }

        /*
        public static bool ExpressionEquals<TValue>(this Expression<Action<TValue>> left, MethodCallExpression right)
        {
            MethodCallExpression leftMethod = (MethodCallExpression)left.Body;

            if (!leftMethod.Method.Equals(right.Method) || leftMethod.Arguments.Count != right.Arguments.Count)
                return false;

            if (leftMethod.Arguments.Count > 0)
            {
                throw new NotSupportedException("Cannot compare expressions that use methods with arguments");
            }

            return true;
        }
         * */

        /*
        public static Func<object, TValue> GetPropertyFuncFromProperty<TValue>(object viewModel, string property)
        {
            Type viewModelType = viewModel.GetType();
            PropertyInfo propInfo = viewModelType.GetProperty(property);
            return p => (TValue) propInfo.GetValue(p,null);
        }
         */

    }
}
