var addMovie = function () {
    var self = this;
    
    self.peoplelist = ko.observableArray();
    
    self.id = ko.observable('');
    self.movietitle = ko.observable('');
    self.plot = ko.observable('');
    self.producer = ko.observable('');
    self.actors = ko.observableArray([
        {actor: ko.observable('')}
    ]);
    self.releasedate = ko.observable('');
    self.showerror = ko.observable(false);
    self.errormessage = ko.observable("Error message here.");
    self.filename = ko.observable('');


    self.init = function (optionalFunction) {
        self.getAllPeople(optionalFunction);
    };

    self.addActors = function () {
        self.actors.push({actor: ko.observable('')});
    };

    self.removeActor = function(actor) {
        self.actors.remove(actor);
    };

    self.getAllPeople = function (optionalFunction) {
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function() {
            if (this.readyState == 4 && this.status === 200) {
                var data = JSON.parse(this.response);
                self.peoplelist(data);
                optionalFunction();
            }
        };
        xhr.open("GET", "/api/people", true);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        xhr.send();
    };

    self.submitForm = function () {

        var errmsg = self.validateFormData();

        if (errmsg === "") {
            var formSubmitPromise = new Promise(function (resolve, reject) {

                var imgInput = document.getElementById("poster");

                if (imgInput !== undefined && imgInput.files !== undefined && imgInput.files.length > 0) {
                    var uploadedFile = imgInput.files[0];
                    var data = new FormData();
                    data.append("file", uploadedFile, uploadedFile.name);

                    var xhr = new XMLHttpRequest();

                    xhr.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status === 200) {
                            self.filename(this.response);
                            resolve(true);
                        }
                    };

                    xhr.open("POST", "/api/image/upload", true);
                    xhr.send(data);
                }
                else {
                    resolve(false);
                }
            });

            formSubmitPromise.then(function(isImageUploaded) {
                self.sendFormData(isImageUploaded);
            });
        }
        else {
            self.errormessage(errmsg);
            self.showerror(true);
        }


        return false;
    };


    self.validateFormData = function () {
        var errorMessage = "";

        if (self.movietitle().length <= 0) {
            errorMessage = "Please enter a title for the movie.";
            return errorMessage;
        }

        if (self.producer() === undefined) {
            errorMessage = "Movie should have a producer, please choose one.";
            return errorMessage;
        }

        if (self.actors()[0].actor() === undefined) {
            errorMessage = "Movie should have at least one actor, please choose one.";
            return errorMessage;
        }

        return errorMessage;
    };

    self.getActorIdsWithoutDuplicates = function() {
        var actorIds = [];
        var actorDict = {};

        for (var i = 0; i < self.actors().length; i++) {
            if (self.actors()[i].actor() !== undefined) {
                actorDict[self.actors()[i].actor()] = 1;
            }
        }

        for (var actor in actorDict) {
            actorIds.push(actor);
        }

        return actorIds;
    };

    self.sendFormData = function(isImageUploaded) {
        
        var actors = self.getActorIdsWithoutDuplicates();

        data = {
            "id": 0,
            "movieTitle" : self.movietitle(),
            "releaseDate": self.releasedate(),
            "plot": self.plot(),
            "posterPath": self.filename(),
            "producerId": self.producer(),
            "actorIds": actors
        };

        data = JSON.stringify(data);
        
        console.log(data);

        var xhr = new XMLHttpRequest();

        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status === 200) {
                window.location.href = "/";
            }
        };

        xhr.open("POST", "/api/movie/save", true);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        xhr.send(data);
    };
    
    self.setFormData = function (movieobj) {
        
        var dateele = document.getElementById("release-date");
        
        self.id(movieobj.id);
        self.movietitle(movieobj.title);
        self.plot(movieobj.plot);
        self.producer(movieobj.producer.id);
        dateele.valueAsDate = new Date(movieobj.releaseDate); 
        self.releasedate(dateele.value);
        self.filename(movieobj.posterPath);
        
        for (var i = 0; i < movieobj.actors.length; i++) {
            var actorele = "actor-" + String(i);
            if (i > 0) {
                self.addActors();
            }
            document.getElementById(actorele).value=movieobj.actors[i].id;
        }        
        
    };

    self.submitUpdateForm = function () {

        var errmsg = self.validateFormData();

        if (errmsg === "") {
            var formSubmitPromise = new Promise(function (resolve, reject) {

                var imgInput = document.getElementById("poster");

                if (imgInput !== undefined && imgInput.files !== undefined && imgInput.files.length > 0) {
                    var uploadedFile = imgInput.files[0];
                    var data = new FormData();
                    data.append("file", uploadedFile, uploadedFile.name);

                    var xhr = new XMLHttpRequest();

                    xhr.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status === 200) {
                            self.filename(this.response);
                            resolve(true);
                        }
                    };

                    xhr.open("POST", "/api/image/upload", true);
                    xhr.send(data);
                }
                else {
                    resolve(false);
                }
            });

            formSubmitPromise.then(function(isImageUploaded) {
                self.sendUpdateFormData(isImageUploaded);
            });
        }
        else {
            self.errormessage(errmsg);
            self.showerror(true);
        }


        return false;
    };

    self.sendUpdateFormData = function(isImageUploaded) {
        
        var actors = self.getActorIdsWithoutDuplicates();

        data = {
            "id": self.id(),
            "movieTitle" : self.movietitle(),
            "releaseDate": self.releasedate(),
            "plot": self.plot(),
            "posterPath": self.filename(),
            "producerId": self.producer(),
            "actorIds": actors
        };

        data = JSON.stringify(data);
        
        console.log(data);

        var xhr = new XMLHttpRequest();

        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status === 200) {
                window.location.href = "/";
            }
        };

        xhr.open("POST", "/api/movie/update", true);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        xhr.send(data);
    };
    
};

var addPerson = function () {
    var self = this;

    self.name = ko.observable('');
    self.sex = ko.observable('');
    self.dob = ko.observable('');
    self.bio = ko.observable('');
    self.sexes = ko.observableArray([{value: 1, text: "Male"}, {value: 1, text: "Female"}, {value: 1, text: "Nonbinary"}]);

    var closeBtn = document.getElementById("closemodal");

    self.savePerson = function () {

        var sexId = self.sex().value;

        data = {
            "name" : self.name(),
            "sex": sexId,
            "dob": self.dob(),
            "bio": self.bio()
        };

        data = JSON.stringify(data);

        console.log(data);

        var xhr = new XMLHttpRequest();

        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status === 200) {
                addMoviePage.refreshPersonList();
                self.clearModal();
                closeBtn.click();
            }
        };

        xhr.open("POST", "/api/people/save", true);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        xhr.send(data);

    };

    self.clearModal = function() {
        self.name('');
        self.sex('');
        self.dob('');
        self.bio('');
    };
};