require 'rubygems'
require 'albacore'
require 'rake/clean'

VERSION = "2.1.1"
OUTPUT = "build"
CONFIGURATION = 'Release'
ASSEMBLY_INFO = 'src/UnityAutoMoq/Properties/AssemblyInfo.cs'
SOLUTION_FILE = 'src/UnityAutoMoq.sln'

Albacore.configure do |config|
	config.log_level = :verbose
	config.msbuild.use :net4
end

desc "Compiles solution and runs unit tests"
task :default => [:clean, :version, :compile, :test, :publish, :package]

desc "Executes all NUnit tests"
task :test => [:nunit]

#Add the folders that should be cleaned as part of the clean task
CLEAN.include(OUTPUT)
CLEAN.include(FileList["src/**/#{CONFIGURATION}"])
CLEAN.include("TestResult.xml")
CLEAN.include("*.nuspec")

desc "Update assemblyinfo file for the build"
assemblyinfo :version => [:clean] do |asm|
	asm.version = VERSION
	asm.company_name = "UnityAutoMoq"
	asm.product_name = "UnityAutoMoq"
	asm.title = "UnityAutoMoq"
	asm.description = "Automocking container using Microsoft Unity and Moq"
	asm.copyright = "Copyright (C) Thomas Pedersen"
	asm.output_file = ASSEMBLY_INFO
end

desc "Compile solution file"
msbuild :compile => [:version] do |msb|
	msb.properties :configuration => CONFIGURATION
	msb.targets :Clean, :Build
	msb.solution = SOLUTION_FILE
end

desc "Gathers output files and copies them to the output folder"
task :publish => [:compile] do
	Dir.mkdir(OUTPUT)
	Dir.mkdir("#{OUTPUT}/binaries")

	FileUtils.cp_r FileList["src/**/#{CONFIGURATION}/*.dll"].exclude(/obj\//).exclude(/.Tests/), "#{OUTPUT}/binaries"
end

desc "Executes NUnit tests"
nunit :nunit => [:compile] do |nunit|
	tests = FileList["src/**/#{CONFIGURATION}/*.Tests.dll"].exclude(/obj\//)

    nunit.command = "src/packages/NUnit.2.5.10.11092/Tools/nunit-console-x86.exe"
	nunit.assemblies = tests
end	

desc "Creates a NuGet packaged based on the UnityAutoMoq.nuspec file"
nugetpack :package => [:publish, :nuspec] do |nuget|
	Dir.mkdir("#{OUTPUT}/nuget")
	
    nuget.command = "tools/nuget.exe"
    nuget.nuspec = "UnityAutoMoq.nuspec"
	nuget.base_folder = "#{OUTPUT}/binaries/"
    nuget.output = "#{OUTPUT}/nuget/"
	nuget.symbols = false
end

desc "Create the nuget package specification"
nuspec do |nuspec|
    nuspec.id ="UnityAutoMoq"
    nuspec.version = VERSION
    nuspec.authors = "Thomas Pedersen (thedersen)"
    nuspec.description = "Automocking container using Microsoft Unity and Moq."
    nuspec.language = "en-US"
    nuspec.projectUrl = "https://github.com/thedersen/UnityAutoMoq"
	nuspec.tags = "unity moq automocking test unittest"
	nuspec.file "UnityAutoMoq.dll", "lib/net35"
    nuspec.dependency "Unity", "2.0"
    nuspec.dependency "Moq", "3.0"
    nuspec.output_file = "UnityAutoMoq.nuspec"
end

desc "Pushes and publishes the NuGet package to nuget.org"
nugetpush :release => [:package] do |nuget|
    nuget.command = "tools/nuget.exe"
    nuget.package = "#{OUTPUT}/nuget/UnityAutoMoq.#{VERSION}.nupkg"
    nuget.create_only = false
end