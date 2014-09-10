var app = angular.module('myApp', []);

app.controller = app.controller('myController', function ($scope, dataService, sortService) {
    $scope.title = "Hello";
    $scope.hoverClass = 'btn';
    $scope.clicked = function() {
        $scope.title += "*";
    };

    $scope.headers = [
        { title: "_id", sortDirection: 0 },
        { title: "Title", sortDirection: 0 },
        { title: "Number", sortDirection: 0 }
    ];

    $scope.data = null;

    dataService.getData(function (dataResponse) {
        $scope.data = dataResponse;
        //$scope.sort = sortService.setSort($scope.headers, $scope.data);
    });

    $scope.sort = function(property) {

        var filter = null;

        for (header in $scope.headers) {
            if ($scope.headers[header] === property) {
                if (property.sortDirection === 0) {
                    property.sortDirection = 1;
                } else {
                    property.sortDirection *= -1;
                }

                filter = "?$orderby=" + property.title + (property.sortDirection === -1 ? " desc" : "");
            }
            else {
                $scope.headers[header].sortDirection = 0;
            }
        };
        dataService.getData(function (dataResponse) {
            $scope.data = dataResponse;
            //$scope.sort = sortService.setSort($scope.headers, $scope.data);
            }, filter);
    };

});

app.factory('sortService', function () {
    return {
        setSort: function (headers, data) {
            var sortProperty;
            var sortDirection;
            var sortIndex;

            var sorter = function () {
                return function (x1, x2) {
                    var result = (x1[sortIndex] < x2[sortIndex]) ? -1 : (x1[sortIndex] > x2[sortIndex]) ? 1 : 0;
                    return result * sortDirection;
                };
            };

            return function (property) {
                if (property === sortProperty) {
                    sortDirection *= -1;
                } else {
                    sortDirection = 1;
                }

                sortProperty = property;

                var findIndex = function () {
                    var returnIndex = -1;
                    for (header in headers) {
                        if (headers[header] === sortProperty) {
                            headers[header].sortDirection = sortDirection;
                            returnIndex = header;
                        } else {
                            headers[header].sortDirection = 0;
                        }
                    }
                    return returnIndex;
                };

                sortIndex = findIndex();

                data = data.sort(sorter());
            }
        }
    };
});

app.factory('dataService', function ($http) {

    return {
        getData: function (callbackFunc, filter) {
            $http({
                method: 'GET',
                url: 'http://localhost:61301/odata/TestClasses' + (filter !== undefined ? filter : "")
            }).success(function (data) {

                var output = [];

                for (d in data.value) {
                    var row = [];
                    for (value in data.value[d]) {
                        row.push(data.value[d][value]);
                    }
                    output.push(row);
                };

                callbackFunc(output);
            }).error(function () {
                alert("error");
            });
        }
    };
});


app.directive('hoverdirective', function () {
    return {
        restrict: 'A',
        link: function (scope, element) {
            element.bind('mouseenter', function () {
                element.css('background-color', 'yellow');
            });
            element.bind('mouseleave', function () {
                element.css('background-color', 'lightgoldenrodyellow');
            });
        }
    };
});

app.directive('margin', function() {
    return {
        scope: { margin: '@' },
        link: function (scope, element) {
            element.css('margin', scope.margin + 'px');
        }
    };
});