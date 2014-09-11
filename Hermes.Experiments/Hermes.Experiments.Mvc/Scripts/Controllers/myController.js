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

                filter = "?$top=10" + "&$orderby=" + property.title + (property.sortDirection === -1 ? " desc" : "");
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