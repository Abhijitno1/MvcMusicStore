using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcMusicStore.Models;

namespace MvcMusicStore.ViewModels
{
    public class AlbumsSearchViewModel
    {
        public IEnumerable<Album> SearchedAlbums { get; set; }
        public struct SearchParams
        {
            public string AlbumTitle;
            public int ArtistId;
            public int GenreId;
        }
        public SearchParams SearchParameters;
    }
}