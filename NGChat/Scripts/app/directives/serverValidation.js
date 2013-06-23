'use strict';

angular
    .module('chat.directives')
    .directive('serverValidation', ['$rootScope', function ($rootScope) {
        return {
            link: function (scope, element, attrs) {
                var form = element.inheritedData('$formController');
                
                if (!form)
                    return;

                scope.$watch(attrs.serverValidation, function (errors) {
                    form.$serverErrors = {};
                    form.$serverValid = true;
                    form.$serverInvalid = false;

                    if (!errors || errors.length == 0)
                        return;

                    form.$serverValid = false;
                    form.$serverInvalid = true;

                    angular.forEach(errors, function (error, i) {
                        form.$serverErrors[error.key] = error.messages;
                    });
                });
            }
        };
    }]);