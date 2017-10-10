// Defining angularjs module
var app = angular.module('userModule', []);
// Defining angularjs Controller and injecting ProductsService
app.controller('userCtrl', function ($scope, $http, UserService) {
    $scope.userData = null;
    // Fetching records from the factory created at the bottom of the script file
    UserService.GetAllRecords().then(function (d) {
        $scope.userData = d.data; // Success
    }, function () {
        alert('Error Occured !!!'); // Failed
    });

    $scope.folder = {};

    $scope.emailArray = [];
    angular.forEach($scope.folder, function (key, value) {
        if (key) $scope.emailArray.push(value);
    });

    var EmailInput = {
        Sender: 'judyzou666@gmail.com',
        Receiver: $scope.emailArray,
        EmailSubject: 'mysubject',
        EmailBody: 'Hello'
    };

    $scope.send = function () {
        angular.forEach($scope.folder, function (key, value) {
            if (key) $scope.emailArray.push(value);
        });
        var EmailInput = {
            Sender: 'judyzou666@gmail.com',
            Receivers: $scope.emailArray,
            EmailSubject: 'mysubject',
            EmailBody: 'Hello'
        };
        //alert($scope.emailArray.length)
        if (EmailInput.Receivers.length > 0) {
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(EmailInput),
                url: 'api/Admin/SendEmail',
                success: function (data, status) {
                    $scope.$apply(function () {
                    alert("Email Sent Successfully !!!");
                });
                },
                error: function (status) { }
                });
        }
    };

    //$scope.User = {
       
    //    FirstName: '',
    //    LastName: '',
    //    Email: ''
       
    //};
    //// Reset product details
    //$scope.clear = function () {
    //    $scope.User.FirstName = '';
    //    $scope.User.LastName = '';
    //    $scope.User.Email = '';
    //    $scope.User.Author = '';
    //}
    ////Add New Item
    //$scope.save = function () {
    //    if ($scope.Book.BookTitle != "" &&
    //    $scope.Book.Price != "" && $scope.Book.Author != "") {
    //        // Call Http request using $.ajax
    //        //$.ajax({
    //        // type: 'POST',
    //        // contentType: 'application/json; charset=utf-8',
    //        // data: JSON.stringify($scope.Book),
    //        // url: 'api/Book/PostBook',
    //        // success: function (data, status) {
    //        // $scope.$apply(function () {
    //        // $scope.bookData.push(data);
    //        // alert("Book Added Successfully !!!");
    //        // $scope.clear();
    //        // });
    //        // },
    //        // error: function (status) { }
    //        //});
    //        // or you can call Http request using $http
    //        $http({
    //            method: 'POST',
    //            url: 'api/Book/PostBook/',
    //            data: $scope.Book
    //        }).then(function successCallback(response) {
    //            // this callback will be called asynchronously

    //            // when the response is available
    //            $scope.bookData.push(response.data);
    //            $scope.clear();
    //            alert("Book Added Successfully !!!");
    //        }, function errorCallback(response) {
    //            // called asynchronously if an error occurs
    //            // or server returns response with an error status.
    //            alert("Error : " + response.data.ExceptionMessage);
    //        });
    //    }
    //    else {
    //        alert('Please Enter All the Values !!');
    //    }
    //};
    //// Edit product details
    //$scope.edit = function (data) {
    //    $scope.Book = { Id: data.Id, BookTitle: data.BookTitle, Price: data.Price, Author: data.Author };
    //}
    //// Cancel product details
    //$scope.cancel = function () {
    //    $scope.clear();
    //}
    //// Update product details
    //$scope.update = function () {
    //    if ($scope.Book.BookTitle != "" &&
    //    $scope.Book.Price != "" && $scope.Book.Author != "") {
    //        $http({
    //            method: 'PUT',
    //            url: 'api/Book/PutBook/' + $scope.Book.Id,
    //            data: $scope.Book
    //        }).then(function successCallback(response) {
    //            $scope.bookData = response.data;
    //            $scope.clear();
    //            alert("Product Updated Successfully !!!");
    //        }, function errorCallback(response) {
    //            alert("Error : " + response.data.ExceptionMessage);
    //        });
    //    }
    //    else {
    //        alert('Please Enter All the Values !!');
    //    }
    //};
    //// Delete product details
    //$scope.delete = function (index) {
    //    $http({
    //        method: 'DELETE',
    //        url: 'api/Book/DeleteBook/' + $scope.bookData[index].Id,
    //    }).then(function successCallback(response) {
    //        $scope.bookData.splice(index, 1);
    //        alert("Book Deleted Successfully !!!");
    //    }, function errorCallback(response) {
    //        alert("Error : " + response.data.ExceptionMessage);
    //    });
    //};
});
// Here I have created a factory which is a popular way to create and configure services.
// You may also create the factories in another script file which is best practice.
app.factory('UserService', function ($http) {
    var fac = {};
    fac.GetAllRecords = function () {
        return $http.get('api/Admin/GetAllUsers');
    }
    return fac;
});