using System;

using System.Diagnostics;
using RxTests;
using Xamarin.Forms;
using Trash.Service.Namur;
using System.Reactive;
using System.Reactive.Linq;
using System.Collections.Generic;
using Trash.Model;
using System.Threading.Tasks;
using Refit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Globalization;
using System.Text;
using System.Reactive.Concurrency;
using ReactiveUI;

namespace RxTests
{
	public class App : Application
	{
		public App ()
		{
			Button butt;

			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Reactive Tests"
						},
						(butt = new Button {
							Text = "test",
						})
					}
				}
			};
		}


		protected virtual ICancellableAlert GetNewAlert ()
		{
			return new DebugCancellableAlert ();
		}

		protected override void OnStart ()
		{
			MainPage.BindingContext = new HomeViewModel (GetNewAlert, RxApp.MainThreadScheduler);

			using (new TrashCollectService ().GetMunicipalitiesObservable ()
				.OnErrorResumeNext (Observable.Empty<List<Municipality>> ())
				.Timeout (TimeSpan.FromSeconds (5))
				.Concat (
				      Observable.Timer (TimeSpan.FromSeconds (4))
					.Select (l => new List<Municipality> ())
			      )
			      //.SubscribeOn(RxApp.TaskpoolScheduler)
			      //.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe (onNext: l => Debug.WriteLine (l.Count), onError: Debug.WriteLine)
			);

			/*
			var fixture = RestService.For<IGitHubApi>("https://api.github.com");
			JsonConvert.DefaultSettings = 
				() => new JsonSerializerSettings { ContractResolver = new SnakeCasePropertyNamesContractResolver() };

			//var result = await fixture.GetUserObservable("benoitjadinon")
			//	.Timeout(TimeSpan.FromSeconds(10));

			var result = fixture.GetUserObservable("benoitjadinon")
				.Delay(TimeSpan.FromSeconds(5))
				.Timeout(TimeSpan.FromSeconds(10))
				.Subscribe (onNext: Debug.WriteLine, onError:Debug.WriteLine);
				*/
		}

		protected override void OnSleep ()
		{
		}

		protected override void OnResume ()
		{
		}
	}

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
	}

	public class UserSearchResult
	{
		public int TotalCount { get; set; }
		public bool IncompleteResults { get; set; }
		public IList<User> Items { get; set; }
	}

	[Headers("User-Agent: Refit Integration Tests")]
	public interface IGitHubApi
	{
		[Get("/users/{username}")]
		Task<User> GetUser(string userName);

		[Get("/users/{username}")]
		IObservable<User> GetUserObservable(string userName);

		[Get("/users/{userName}")]
		IObservable<User> GetUserCamelCase(string userName);

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

