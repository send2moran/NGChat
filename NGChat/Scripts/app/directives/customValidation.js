'use strict';

angular
    .module('chat.directives')
    .directive('customValidation', ['$rootScope', function ($rootScope) {
        return {
            link: function (scope, element, attrs) {
                var form = element.inheritedData('$formController');

                form._server = {
                    valid: true,
                    invalid: false
                };
                form._hasAnyError = true;

                function resetServerErrors() {
                    form._server.valid = true;
                    form._server.invalid = false;
                    form._server.errors = [];
                }

                scope.$watch(attrs.customValidation, function (errors) {
                    resetServerErrors();

                    if (!errors || errors.length == 0) {
                        if (form.$valid || form.$pristine)
                            form._hasAnyError = false;

                        return;
                    }

                    form._hasAnyError = true;

                    angular.forEach(errors, function (error, i) {
                        form._server.errors[error.key] = error.messages;
                    });
                });

                scope.$watch(function () {
                    return form.$valid;
                }, function (isValid) {
                    if (!isValid && form.$dirty)
                        form._hasAnyError = true;
                    else if (form._server.valid)
                        form._hasAnyError = false;
                });
            }
        };
    }]);
