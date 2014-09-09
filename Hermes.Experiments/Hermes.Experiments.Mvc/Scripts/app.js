var app = angular.module('myApp', []);

app.controller = app.controller('myController', function($scope) {
    $scope.title = "Hello";
    $scope.hoverClass = 'btn';
    $scope.clicked = function() {
        $scope.title += "*";
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