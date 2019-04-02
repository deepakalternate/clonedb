var addMoviePage = (function(){
    
    var vmAddMovie = new addMovie();
    var vmAddPerson = new addPerson();
    
    var init = function () {
        var addMovieForm = document.getElementById("add-movie-form");
        var addPersonForm = document.getElementById("add-actor-modal");
        if (addMovieForm) {
            vmAddMovie.init(checkForEditPage);
            ko.applyBindings(vmAddMovie, addMovieForm);
            if (addPersonForm) {
                ko.applyBindings(vmAddPerson, addPersonForm);
            }
        }
    };
    
    var __noop__ = function () { };
    
    var refreshPersonList = function () {
        vmAddMovie.getAllPeople(__noop__);
    };
    
    var checkForEditPage = function () {
        var movieData = document.getElementById("serialized-movie");
        
        if (movieData) {
            var serializedMovie = JSON.parse(movieData.getAttribute("data-json"));
            vmAddMovie.setFormData(serializedMovie);
        }
    };
    
    return {
        init: init,
        refreshPersonList: refreshPersonList,
        vmAddMovie: vmAddMovie
    };
})();

addMoviePage.init();
