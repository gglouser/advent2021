Advent2021 C# Solutions
=======================

Grant Glouser <gglouser@gmail.com>


## Project setup

Commands to create top-level solution and sub-projects:
```
dotnet new sln -o csharp
cd csharp
dotnet new classlib -o Advent2021.Solutions
dotnet sln add .\Advent2021.Solutions\Advent2021.Solutions.csproj
dotnet new xunit -o Advent2021.Tests
dotnet add .\Advent2021.Tests\Advent2021.Tests.csproj reference .\Advent2021.Solutions\Advent2021.Solutions.csproj
dotnet sln add .\Advent2021.Tests\Advent2021.Tests.csproj
rm .\Advent2021.Solutions\Class1.cs
rm .\Advent2021.Tests\UnitTest1.cs
```

Put inputs in Advent2021.Tests/inputs/ and
add the following to Advent2021.Tests/Advent2021.Tests.csproj:

```
  <ItemGroup>
    <Content Include="inputs\*.txt">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
```
