﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using Atlas.Models.Public;
using Microsoft.AspNetCore.Components;

namespace Atlas.Client.Components.Themes
{
    public abstract class PostComponent : ThemeComponentBase
    {
        [Parameter] public Guid ForumId { get; set; }
        [Parameter] public Guid? TopicId { get; set; }

        protected PostPageModel Model { get; set; }

        protected string TitleText => TopicId != null ? Loc["Update Topic"] : Loc["Create New Topic"];
        protected string ButtonText => TopicId != null ? Loc["Update"] : Loc["Save"];

        protected bool DisplayPage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var requestUri = TopicId != null
                ? $"api/public/topics/{ForumId}/edit-topic/{TopicId.Value}"
                : $"api/public/topics/{ForumId}/new-topic";

            try
            {
                Model = await ApiService.GetFromJsonAsync<PostPageModel>(requestUri);
                DisplayPage = true;
            }
            catch (Exception)
            {
                Model = new PostPageModel();
                DisplayPage = false;
            }
        }

        protected async Task SaveAsync()
        {
            var requestUri = TopicId != null
                ? "api/public/topics/update-topic"
                : "api/public/topics/create-topic";

            var response = await ApiService.PostAsJsonAsync(requestUri, Model);
            var content = await response.Content.ReadAsStringAsync();
            var topicId = JsonSerializer.Deserialize<Guid>(content);

            Navigation.NavigateTo($"/forum/{ForumId}/{topicId}");
        }

        protected void Cancel()
        {
            var cancelUri = TopicId != null
                ? $"/forum/{ForumId}/{TopicId.Value}"
                : $"/forum/{ForumId}";

            Navigation.NavigateTo(cancelUri);
        }
    }
}