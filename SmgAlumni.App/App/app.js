'use strict';

var app = angular.module('app', ['ngSanitize', 'ui.bootstrap', 'ui.router', 'angular-loading-bar', 'ngDialog']);



app.config([
    '$stateProvider', '$httpProvider', '$urlRouterProvider', 'ngDialogProvider',
    function ($stateProvider, $httpProvider, $urlRouterProvider, ngDialogProvider) {

        //================================================
        //add an request interceptor
        //================================================
        $httpProvider.interceptors.push('authInterceptor');

        //================================================
        // Make urls case insensitive
        //================================================
        $urlRouterProvider.rule(function ($injector, $location) {
            var path = $location.path(), normalized = path.toLowerCase();
            if (path != normalized) {
                $location.replace().path(normalized);
            }
        });

        //================================================
        // Dialog
        //================================================
        ngDialogProvider.setDefaults({
            className: 'ngdialog-theme-default',
            plain: false,
            showClose: true,
            closeByDocument: true,
            closeByEscape: true,
            appendTo: false,
        });

        //================================================
        // Routes
        //================================================ 
        $urlRouterProvider.otherwise("/");

        $stateProvider.state('menu',
        {
            templateUrl: '/App/templates/menu.html'
        });

        $stateProvider.state('home', {
            url: '/',
            templateUrl: '/App/templates/home/home.html'
            //controller: 'homeController'
        });
    
       
        $stateProvider.state('login', {
            url: '/account/login',
            templateUrl: '/App/templates/account/login.html',
            controller: 'loginController',
        });
        $stateProvider.state('register', {
            url: '/account/register',
            templateUrl: '/App/templates/account/register.html',
            controller: 'registerController',
        });
        $stateProvider.state('successfullregistration', {
            url: '/account/register/success',
            templateUrl: '/App/templates/account/registersuccess.html'
        });
        $stateProvider.state('forgotpassword', {
            url: '/account/forgotpassword',
            templateUrl: '/App/templates/account/forgotPassword.html',
            controller: 'forgotPasswordController'
        });
        $stateProvider.state('resetpassword', {
            url: '/account/resetpassword?guid&email',
            templateUrl: '/App/templates/account/resetPassword.html',
            controller: 'resetPasswordController'
        });
        $stateProvider.state('manageaccount', {
            url: '/account/manageaccount',
            templateUrl: '/App/templates/account/manageAccount.html',
            controller: 'manageAccountController',
            authenticate: true,
            resolve: {
                managedata: function (accountService) {
                    return accountService.getAccountData();
                }
            }
        });
        $stateProvider.state('changepassword', {
            url: '/account/changepassword',
            templateUrl: '/App/templates/account/changePassword.html',
            controller: 'changePasswordController',
            authenticate: true
        });
    }]).run(['$rootScope', '$state', function ($rootScope, $state) {
        $rootScope.$on("$stateChangeStart", function (event, toState) {
            if ((toState.authenticate) &&
                (sessionStorage.authenticationData == undefined ||
                sessionStorage.authenticationData == '')) {
                $state.transitionTo('login', { returnUrl: toState.name });
                event.preventDefault();
            }
        });
    }
    ]);
