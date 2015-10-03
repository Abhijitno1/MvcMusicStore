using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;
using System;
using System.Diagnostics;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        MusicStoreEntities storeDB = new MusicStoreEntities();

        public ActionResult Index()
        {
            // Get most popular albums
            var albums = GetTopSellingAlbums(5);

            return View(albums);
        }

        public ActionResult AlbumsSearch(int? page, string sort, string sortdir)
        {
            //Debug.WriteLine("page = " + (page.HasValue ? "" : page.ToString())); 
            var albumSearchviewModel = new ViewModels.AlbumsSearchViewModel();
            if (page.HasValue || sort != null)
            {
                albumSearchviewModel = (ViewModels.AlbumsSearchViewModel)Session["AlbumSearchParameters"];
                string genreId = albumSearchviewModel.SearchParameters.GenreId.ToString();
                string artistId = albumSearchviewModel.SearchParameters.ArtistId.ToString();
                PopulateDropdowns(genreId, artistId);
            }
            else
            {
                albumSearchviewModel.SearchedAlbums = new List<Album>();
                PopulateDropdowns(string.Empty, string.Empty);
            }
            return View(albumSearchviewModel);
        }

        [HttpPost]
        public ActionResult AlbumsSearch(FormCollection searchParams)
        {
            //Debug.WriteLine("Form Keys: " + string.Join(",", searchParams.AllKeys));
            int genreId = 0, artistId = 0;
            string title = string.Empty;

            var albums = storeDB.Albums.AsQueryable();
            if (searchParams["GenreId"] != "0")
            {
                genreId = Convert.ToInt32(searchParams["GenreId"]);
                albums = albums.Where(album => album.GenreId == genreId);
            }
            if (searchParams["ArtistId"] != "0")
            {
                artistId = Convert.ToInt32(searchParams["ArtistId"]);
                albums = albums.Where(album => album.ArtistId == artistId);
            }
            if (searchParams["SearchParameters.AlbumTitle"].ToString().Trim() != string.Empty)
            {
                title = searchParams["SearchParameters.AlbumTitle"].ToString().Trim().ToLower();
                albums = albums.Where(album => album.Title.ToLower().Contains(title));
            }

            PopulateDropdowns(genreId.ToString(), artistId.ToString());
            var albumSearchviewModel = new ViewModels.AlbumsSearchViewModel();
            albumSearchviewModel.SearchedAlbums = albums;
            albumSearchviewModel.SearchParameters.GenreId = genreId;
            albumSearchviewModel.SearchParameters.ArtistId = artistId;
            albumSearchviewModel.SearchParameters.AlbumTitle = title;
            Session["AlbumSearchParameters"] = albumSearchviewModel;
            return View(albumSearchviewModel);
        }

        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count

            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }

        private void PopulateDropdowns(string selectedGenre, string selectedArtist)
        {
            var genresList = new SelectList(storeDB.Genres, "GenreId", "Name", selectedGenre).ToList();
            genresList.Insert(0, new SelectListItem { Text = "", Value = "0" });
            var artistList = new SelectList(storeDB.Artists, "ArtistId", "Name", selectedArtist).ToList();
            artistList.Insert(0, new SelectListItem { Text = "", Value = "0" });
            ViewBag.GenreId = genresList;
            ViewBag.ArtistId = artistList;
        }

    }
}