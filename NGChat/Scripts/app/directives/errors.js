'use strict';

angular
    .module('chat.directives')
    .directive('clientErrors', ['$rootScope', function ($rootScope) {
        return {
            replace: true,
            restrict: 'EA',
            transclude: true,
            template: '<div class="errors" data-ng-transclude=""></div>',
            link: function (scope, element, attrs) {
            }
        };
    }]);