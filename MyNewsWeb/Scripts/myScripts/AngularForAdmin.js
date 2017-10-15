// Defining angularjs module
var app = angular.module('appModule', ['angularjs-dropdown-multiselect', 'smart-table']);
// Defining angularjs Controller and injecting ProductsService
app.controller('userCtrl', function ($scope, $http, UserService) {
    $scope.predicateUsers = ['Email', 'FirstName', 'LastName'];
    $scope.selectedPredicateUser = $scope.predicateUsers[0];
    $scope.itemsByPage = 5;

    $scope.userRolesOptions = [{
        id: 1,
        label: "Admin"
    }, {
        id: 2,
        label: "Journalist"
    }, {
        id: 3,
        label: "Public User"
    }];

    $scope.selected_baseline_settings = {
        smartButtonMaxItems: 2,
        displayProp: 'label',
        idProp: 'label',
    };

    $scope.selected_baselines_customTexts = { buttonDefaultText: 'Select User Roles' };

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
                error: function (status) {
                    alert("Error!!Email Was not sent succesfully");
                }
                });
        } else {
            alert("Please select at least one user in the above table!");
        }
    };

    $scope.User = {
        Id: '',
        Email: '',
        FirstName: '',
        LastName: '',
        Password: '',
        PhoneNumber: '',
        DateOfBirth: '',
        Street: '',
        Suburb: '',
        State: '',
        PostCode: '',
        UserRoles: []
    };
    // Reset user details
    $scope.clearUser = function () {
        $scope.User.Email = '';
        $scope.User.FirstName = '';
        $scope.User.LastName = '';
        $scope.User.Password = '';
        $scope.User.PhoneNumber = '';
        $scope.User.DateOfBirth = '';
        $scope.User.Street = '';
        $scope.User.Suburb = '';
        $scope.User.State = '';
        $scope.User.PostCode = '';
        $scope.User.UserRoles = [];
    }
    //Add New User
    $scope.saveUser = function () {
        if ($scope.User.Email != "" &&
        $scope.User.FirstName != "" && $scope.User.LastName != "" && $scope.User.Password != "") {
            $http({
                method: 'POST',
                url: 'api/Admin/AddUser/',
                data: $scope.User
            }).then(function successCallback(response) {
                // this callback will be called asynchronously

                // when the response is available
                $scope.userData.push(response.data);
                $scope.clearUser();
                alert("User Added Successfully !!!");
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                alert("Error : " + response.data.ExceptionMessage);
            });
        }
        else {
            alert('Please Enter All the Values !!');
        }
    };

    //Edit sude detail
    $scope.editUser = function (data) {
        $scope.User = {
            Id: data.Id,
            Email: data.Email,
            FirstName: data.FirstName,
            LastName: data.LastName,
            PhoneNumber: data.PhoneNumber,
            DateOfBirth: data.DateOfBirth,
            Street: data.Street,
            Suburb: data.Suburb,
            State: data.State,
            PostCode: data.PostCode,
            UserRoles: data.UserRoles
        };
    }

    //// Cancel product details
    //$scope.cancel = function () {
    //    $scope.clear();
    //}
    // Update product details
    $scope.updateUser = function () {
        if ($scope.User.Email != "" &&
        $scope.User.FirstName != "" && $scope.User.LastName != "" && $scope.User.Password != "") {            
            $http({
                method: 'PUT',
                url: 'api/Admin/UpdateUser/' + $scope.User.Id,
                data: $scope.User
            }).then(function successCallback(response) {
                $scope.userData = response.data;
                $scope.clearUser();
                alert("User Updated Successfully !!!");
            }, function errorCallback(response) {
                alert("Error : " + response.data.ExceptionMessage);
            });
        }
        else {
            alert('Please Enter All the Values !!');
        }
    };
    // Delete User details
    $scope.deleteUser = function () {
        $http({
            method: 'DELETE',
            url: 'api/Admin/DeleteUser/' + $scope.User.Id,
        }).then(function successCallback(response) {
            $scope.userData = response.data;
            $scope.clearUser();
            alert("User Deleted Successfully !!!");
        }, function errorCallback(response) {
            alert("Error : " + response.data.ExceptionMessage);
        });
    };
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