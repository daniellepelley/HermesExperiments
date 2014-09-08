var app = angular.module('myApp', []);

app.controller = app.controller('myController', function($scope) {
    $scope.title = "Hello";
    $scope.hoverClass = 'btn';
});


app.directive('hoverClass', function() {
    return {
        restrict: 'A',
        scope: {
            hoverClass: '@'
        },
        link: function(scope, element) {
            element.on('mouseenter', function() {
                element.css('background-color', 'yellow');

                //$(element).toggle("fade");

                //element.addClass(scope.hoverClass);
            });
            element.on('mouseleave', function() {
                element.css('background-color', 'lightgoldenrodyellow');
                //element.removeClass(scope.hoverClass);
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