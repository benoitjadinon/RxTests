using System;
using Refit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace GitHub
{
	public class User
	{
		public string Login { get; set; }
		public int Id { get; set; }
		public string AvatarUrl { get; set; }
		public string GravatarId { get; set; }
		public string Url { get; set; }
		public string HtmlUrl { get; set; }
		public string FollowersUrl { get; set; }
		public string FollowingUrl { get; set; }
		public string GistsUrl { get; set; }
		public string StarredUrl { get; set; }
		public string SubscriptionsUrl { get; set; }
		public string OrganizationsUrl { get; set; }
		public string ReposUrl { get; set; }
		public string EventsUrl { get; set; }
		public string ReceivedEventsUrl { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string Company { get; set; }
		public string Blog { get; set; }
		public string Location { get; set; }
		public string Email { get; set; }
		public bool? Hireable { get; set; }
		public string Bio { get; set; }
		public int PublicRepos { get; set; }
		public int Followers { get; set; }
		public int Following { get; set; }
		public string CreatedAt { get; set; }
		public string UpdatedAt { get; set; }
		public int PublicGists { get; set; }

		public override string ToString ()
		{
			return string.Format ("[User: Login={0}, Id={1}, AvatarUrl={2}, GravatarId={3}, Url={4}, HtmlUrl={5}, FollowersUrl={6}, FollowingUrl={7}, GistsUrl={8}, StarredUrl={9}, SubscriptionsUrl={10}, OrganizationsUrl={11}, ReposUrl={12}, EventsUrl={13}, ReceivedEventsUrl={14}, Type={15}, Name={16}, Company={17}, Blog={18}, Location={19}, Email={20}, Hireable={21}, Bio={22}, PublicRepos={23}, Followers={24}, Following={25}, CreatedAt={26}, UpdatedAt={27}, PublicGists={28}]", Login, Id, AvatarUrl, GravatarId, Url, HtmlUrl, FollowersUrl, FollowingUrl, GistsUrl, StarredUrl, SubscriptionsUrl, OrganizationsUrl, ReposUrl, EventsUrl, ReceivedEventsUrl, Type, Name, Company, Blog, Location, Email, Hireable, Bio, PublicRepos, Followers, Following, CreatedAt, UpdatedAt, PublicGists);
		}
	}

	public class UserSearchResult
	{
		public int TotalCount { get; set; }
		public bool IncompleteResults { get; set; }
		public IList<User> Items { get; set; }
	}

	public class Repo
	{
		public int Id { get; set; }
		public bool? Private { get; set; }
		public bool? Fork { get; set; }
		public string Name { get; set; }
		public string FullName { get; set; }
		public string Url { get; set; }
		public string HtmlUrl { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Repo: Id={0}, Private={1}, Fork={2}, Name={3}, FullName={4}, Url={5}, HtmlUrl={6}]", Id, Private, Fork, Name, FullName, Url, HtmlUrl);
		}
	}

	[Headers("User-Agent: Refit Integration Tests")]
	public interface IGitHubApi
	{
		[Get("/users/{username}")]
		Task<User> GetUser(string userName);

		[Get("/users/{username}")]
		IObservable<User> GetUserObservable(string username);

		[Get("/orgs/{orgname}/members")]
		Task<List<User>> GetOrgMembers(string orgName);

		[Get("/search/users")]
		Task<UserSearchResult> FindUsers(string q);

		[Get("/")]
		Task<HttpResponseMessage> GetIndex();

		[Get("/")]
		IObservable<string> GetIndexObservable();

		[Get("/give-me-some-404-action")]
		Task NothingToSeeHere();

		[Get ("/users/{username}/repos?type=owner&sort=updated&direction=desc")]
		IObservable<List<Repo>> GetReposOwned(string username);
	}

	public class GitHubApi : IGitHubApi 
	{
		readonly IGitHubApi api;

		public GitHubApi ()
		{
			api = RestService.For<IGitHubApi>("https://api.github.com");
			JsonConvert.DefaultSettings = 
				() => new JsonSerializerSettings { ContractResolver = new SnakeCasePropertyNamesContractResolver() };
		}

		#region IGitHubApi implementation

		public async Task<User> GetUser (string userName)
		{
			return await api.GetUser (userName);
		}

		public IObservable<User> GetUserObservable (string userName)
		{
			return api.GetUserObservable (userName);
		}

		public async Task<List<User>> GetOrgMembers (string orgName)
		{
			return await api.GetOrgMembers (orgName);
		}

		public async Task<UserSearchResult> FindUsers (string q)
		{
			return await api.FindUsers (q);
		}

		public async Task<HttpResponseMessage> GetIndex ()
		{
			return await api.GetIndex ();
		}

		public IObservable<string> GetIndexObservable ()
		{
			return api.GetIndexObservable ();
		}

		public Task NothingToSeeHere ()
		{
			return api.NothingToSeeHere ();
		}
			
		public IObservable<List<Repo>> GetReposOwned (string username)
		{
			return api.GetReposOwned (username);
		}

		#endregion
	}

	public class DeliminatorSeparatedPropertyNamesContractResolver : DefaultContractResolver
	{
		readonly string separator;

		protected DeliminatorSeparatedPropertyNamesContractResolver(char separator)
			: base(true)
		{
			this.separator = separator.ToString();
		}

		protected override string ResolvePropertyName(string propertyName)
		{
			var parts = new List<string>();
			var currentWord = new StringBuilder();

			foreach (var c in propertyName.ToCharArray()) {
				if (Char.IsUpper(c) && currentWord.Length > 0) {
					parts.Add(currentWord.ToString());
					currentWord.Clear();
				}

				currentWord.Append(char.ToLower(c));
			}

			if (currentWord.Length > 0) {
				parts.Add(currentWord.ToString());
			}

			return String.Join(separator, parts.ToArray());
		}
	}

	public class SnakeCasePropertyNamesContractResolver : DeliminatorSeparatedPropertyNamesContractResolver
	{
		public SnakeCasePropertyNamesContractResolver() : base('_') { }
	}
}