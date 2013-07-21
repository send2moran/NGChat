'use strict';

angular
    .module('chat.directives')
    .directive('selectText', ['$timeout', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                $timeout(function () {
                    element.focus();
                    element.select();
                }, 0);
            }
        };
    }]);
