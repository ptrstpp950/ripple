﻿using System;
using System.Collections.Generic;
using System.IO;
using FubuCore;
using FubuCore.Descriptions;
using ripple.MSBuild;

namespace ripple.New
{
	public class Project : DescribesItself
	{
		private readonly IList<NugetDependency> _dependencies = new List<NugetDependency>();
		private readonly Lazy<CsProjFile> _csProj;

		public Project(string filePath)
		{
			FilePath = filePath;
			Name = Path.GetFileNameWithoutExtension(filePath);
			
			_csProj = new Lazy<CsProjFile>(() => new CsProjFile(filePath));
		}

		public string Name { get; private set; }
		public string FilePath { get; private set; }
		public CsProjFile CsProj { get { return _csProj.Value; } }
		public Solution Solution { get; set; }

		public IEnumerable<NugetDependency> Dependencies
		{
			get { return _dependencies; }
			set
			{
				_dependencies.Clear();
				_dependencies.AddRange(value);
			}
		}

		public void AddDependency(NugetDependency dependency)
		{
			_dependencies.Fill(dependency);
		}

		public void RemoveDependency(NugetDependency dependency)
		{
			_dependencies.Remove(dependency);
		}

		protected bool Equals(Project other)
		{
			return string.Equals(FilePath, other.FilePath);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Project)obj);
		}

		public override int GetHashCode()
		{
			return FilePath.GetHashCode();
		}

		public void Describe(Description description)
		{
			description.Title = "Project \"{0}\"".ToFormat(Name);
			description.ShortDescription = FilePath;

			var list = description.AddList("Dependencies", Dependencies);
			list.Label = "Dependencies";
		}
	}
}