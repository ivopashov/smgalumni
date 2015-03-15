﻿'use strict';

var app = angular.module('app', ['ngSanitize', 'ui.bootstrap', 'ui.router', 'angular-loading-bar', 'ngDialog', 'angularFileUpload']);



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
        $stateProvider.state('homeauth', {
            url: '/home',
            templateUrl: '/App/templates/home/homeAuth.html',
            authenticate: true
        });
        $stateProvider.state('account', {
            url: '/account',
            templateUrl: '/App/templates/account/accountBase.html',
            authenticate: true
        });
        $stateProvider.state('account.manageaccount', {
            url: '/manageaccount',
            templateUrl: '/App/templates/account/manageAccount.html',
            controller: 'manageAccountController',
            authenticate: true
        });
        $stateProvider.state('account.changepassword', {
            url: '/changepassword',
            templateUrl: '/App/templates/account/changePassword.html',
            controller: 'changePasswordController',
            authenticate: true
        });
        //admin
        $stateProvider.state('admin', {
            url: '/admin',
            templateUrl: '/App/templates/admin/adminBase.html',
            authenticate: true
        });
        $stateProvider.state('admin.verifyusers', {
            url: '/verifyusers',
            templateUrl: '/App/templates/admin/verifyUsers.html',
            controller: 'verifyUsersController',
            authenticate: true
        });
    }]).run(['$rootScope', '$state', function ($rootScope, $state) {
        $rootScope.$on("$stateChangeStart", function (event, toState) {
            if ((toState.authenticate) &&
                !sessionStorage.authenticationData) {
                $state.transitionTo('login', { returnUrl: toState.name });
                event.preventDefault();
            }
        });
    }
    ]);
