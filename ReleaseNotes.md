# 0.4.7 (04 June 2022)
- [#33](https://github.com/StefH/FluentBuilder/pull/33) - Fix StackOverflow when there is a constructor with itself contributed by [StefH](https://github.com/StefH)
- [#34](https://github.com/StefH/FluentBuilder/pull/34) - Implement Func and Action [enhancement] contributed by [StefH](https://github.com/StefH)
- [#28](https://github.com/StefH/FluentBuilder/issues/28) - Generating a builder for a property of a type where the ctor with the least parameters expects the property type itself causes a stack overflow
- [#32](https://github.com/StefH/FluentBuilder/issues/32) - Unable to use `Func` and `Action` with the builder methods

# 0.4.6 (20 May 2022)
- [#31](https://github.com/StefH/FluentBuilder/pull/31) - Fix generation failing when the target type has at least one ctor with parameters contributed by [iam-mholle](https://github.com/iam-mholle)
- [#30](https://github.com/StefH/FluentBuilder/issues/30) - Builder generation fails if the target type has at least one ctor with parameters [bug]

# 0.4.5 (16 May 2022)
- [#29](https://github.com/StefH/FluentBuilder/pull/29) - Support IReadOnlyCollection and ReadOnlyCollection [enhancement] contributed by [StefH](https://github.com/StefH)
- [#27](https://github.com/StefH/FluentBuilder/issues/27) - Wrong initialization is generated for ReadOnlyCollection&lt;&gt; properties [bug]

# 0.4.4 (27 April 2022)
- [#26](https://github.com/StefH/FluentBuilder/pull/26) - Replace Array.Empty by new T[0] (supporting .NET45) [bug] contributed by [StefH](https://github.com/StefH)

# 0.4.3 (27 April 2022)
- [#24](https://github.com/StefH/FluentBuilder/pull/24) - Use correct default value for reference types [bug] contributed by [StefH](https://github.com/StefH)
- [#25](https://github.com/StefH/FluentBuilder/pull/25) - Create correct default() statement (use New constructor when possible) [enhancement] contributed by [StefH](https://github.com/StefH)
- [#22](https://github.com/StefH/FluentBuilder/issues/22) - The type or namespace name 'UserBuilder' does not exist in the namespace 'FluentBuilder' (are you missing an assembly reference?) [bug]

# 0.4.2 (03 April 2022)
- [#23](https://github.com/StefH/FluentBuilder/pull/23) - Add support for file scoped namespaces [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.4.0 (18 February 2022)
- [#21](https://github.com/StefH/FluentBuilder/pull/21) - Use full type name + change namespace from builders [bug] contributed by [StefH](https://github.com/StefH)

# 0.3.2 (16 February 2022)
- [#20](https://github.com/StefH/FluentBuilder/pull/20) - Do not inherit IEnumerableBuilder anymore [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.3.1 (14 February 2022)
- [#19](https://github.com/StefH/FluentBuilder/pull/19) - Generate Error.g.cs file in case no public and parameterless constructor is present in the target class contributed by [StefH](https://github.com/StefH)

# 0.3.0 (12 February 2022)
- [#18](https://github.com/StefH/FluentBuilder/pull/18) - Add support to use this FluentBuilder for all classes [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.2.5 (08 February 2022)
- [#17](https://github.com/StefH/FluentBuilder/pull/17) - For interface or array, no cast is needed [bug] contributed by [StefH](https://github.com/StefH)

# 0.2.4 (06 February 2022)
- [#16](https://github.com/StefH/FluentBuilder/pull/16) - Fix support for normal Dictionary&lt;,&gt; [bug] contributed by [StefH](https://github.com/StefH)

# 0.2.3 (01 February 2022)
- [#15](https://github.com/StefH/FluentBuilder/pull/15) - Refactor IEumerable builders [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.2.2 (31 January 2022)
- [#14](https://github.com/StefH/FluentBuilder/pull/14) - Add support for IDictionary [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.2.1 (30 January 2022)
- [#12](https://github.com/StefH/FluentBuilder/pull/12) - Add support for Array and IEnumerable [enhancement] contributed by [StefH](https://github.com/StefH)
- [#13](https://github.com/StefH/FluentBuilder/pull/13) - Skip Dictionary for IEnumerableBuilder logic [bug] contributed by [StefH](https://github.com/StefH)

# 0.1.2 (20 January 2022)
- [#11](https://github.com/StefH/FluentBuilder/pull/11) - Also add 'useObjectInitializer' in With* methods [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.1.1 (15 January 2022)
- [#10](https://github.com/StefH/FluentBuilder/pull/10) - Remove runtime check for parameterless constructor [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.1.0 (13 January 2022)
- [#9](https://github.com/StefH/FluentBuilder/pull/9) - Add support for calling DefaultConstructor instead of using ObjectInitializer  [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.0.11 (10 August 2021)
- [#8](https://github.com/StefH/FluentBuilder/pull/8) - Add DevelopmentDependency = true to csproj [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.0.8 (06 August 2021)
- [#7](https://github.com/StefH/FluentBuilder/pull/7) - Add support for generics + update generated filename [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.0.7 (05 August 2021)
- [#5](https://github.com/StefH/FluentBuilder/pull/5) - Add autogenerated header [enhancement] contributed by [StefH](https://github.com/StefH)
- [#6](https://github.com/StefH/FluentBuilder/pull/6) - Only emit #nullable when nullable is supported (&gt;= LanguageVersion.CS&#8230; [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.0.5 (05 August 2021)
- [#4](https://github.com/StefH/FluentBuilder/pull/4) - Add support for Action&lt;builder&gt; [enhancement] contributed by [StefH](https://github.com/StefH)
- [#3](https://github.com/StefH/FluentBuilder/issues/3) - Add WithProperty overloads that accepts the property's builder when available [enhancement]

# 0.0.4 (21 July 2021)
- [#2](https://github.com/StefH/FluentBuilder/pull/2) - Make project Unit-Testable [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.0.2 (19 July 2021)
- [#1](https://github.com/StefH/FluentBuilder/pull/1) - Update Lazy constructor to fix issues with .NET 4.5 [bug] contributed by [StefH](https://github.com/StefH)

