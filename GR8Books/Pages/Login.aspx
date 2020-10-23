<%@ Page Title="Login" ValidateRequest="false" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="GR8Books.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root {
            --input-padding-x: 1.5rem;
            --input-padding-y: .75rem;
        }


        .card-signin {
            border: 0;
            border-radius: 1rem;
            box-shadow: 0 0.5rem 1rem 0 rgba(0, 0, 0, 0.1);
        }

            .card-signin .card-title {
                margin-bottom: 2rem;
                font-weight: 300;
                font-size: 1.5rem;
            }

            .card-signin .card-body {
                padding: 2rem;
            }

        .form-signin {
            width: 100%;
        }

        .form-label-group {
            position: relative;
            margin-bottom: 1rem;
        }

            .form-label-group input {
                height: auto;
                border-radius: 2rem;
            }

            .form-label-group > input,
            .form-label-group > label {
                padding: var(--input-padding-y) var(--input-padding-x);
            }

            .form-label-group > label {
                position: absolute;
                top: 0;
                left: 0;
                display: block;
                width: 100%;
                margin-bottom: 0;
                /* Override default `<label>` margin */
                line-height: 1.5;
                color: #495057;
                border: 1px solid transparent;
                border-radius: .25rem;
                transition: all .1s ease-in-out;
            }

            .form-label-group input::-webkit-input-placeholder {
                color: transparent;
            }

            .form-label-group input:-ms-input-placeholder {
                color: transparent;
            }

            .form-label-group input::-ms-input-placeholder {
                color: transparent;
            }

            .form-label-group input::-moz-placeholder {
                color: transparent;
            }

            .form-label-group input::placeholder {
                color: transparent;
            }

            .form-label-group input:not(:placeholder-shown) {
                padding-top: calc(var(--input-padding-y) + var(--input-padding-y) * (2 / 3));
                padding-bottom: calc(var(--input-padding-y) / 3);
            }

                .form-label-group input:not(:placeholder-shown) ~ label {
                    padding-top: calc(var(--input-padding-y) / 3);
                    padding-bottom: calc(var(--input-padding-y) / 3);
                    font-size: 12px;
                    color: #777;
                }

        .btn-google {
            color: white;
            background-color: #ea4335;
            font-size: 80%;
            border-radius: 5rem;
            letter-spacing: .1rem;
            font-weight: bold;
            padding: 1rem;
            transition: all 0.2s;
        }

        .btn-facebook {
            color: white;
            background-color: #3b5998;
            font-size: 80%;
            border-radius: 5rem;
            letter-spacing: .1rem;
            font-weight: bold;
            padding: 1rem;
            transition: all 0.2s;
        }

        .btn-signin {
            font-size: 80%;
            border-radius: 5rem;
            letter-spacing: .1rem;
            font-weight: bold;
            padding: 1rem;
            transition: all 0.2s;
        }
    </style>

    <script src="../Scripts/jquery.slim.min.js"></script>
    <br />
    <div class="row mt-5">
        <div class="col-sm-9 col-md-7 col-lg-5 mx-auto">
            <div class="card card-signin my-5">
                <div class="card-body">
                    <h5 class="card-title text-center">Sign In</h5>
                    <form class="form-signin">
                        <div class="form-label-group">
                            <input type="email" id="txtEMail" class="form-control" placeholder="Email address" runat="server" required>
                            <label for="inputEmail">Email address</label>
                        </div>

                        <div class="form-label-group">
                            <input type="password" id="txtPassword" class="form-control" placeholder="Password" runat="server" required>
                            <label for="inputPassword">Password</label>
                        </div>

                        <div class="custom-control custom-checkbox mb-3">
                            <input type="checkbox" class="custom-control-input" id="customCheck1">
                            <label class="custom-control-label" for="customCheck1">Remember password</label>
                        </div>
                        <asp:Button Text="Sign in" class="btn btn-lg btn-primary btn-block text-uppercase btn-signin" ID="btnLogin" runat="server" />
                        <div class="row mt-2">
                            <div class="col">
                                <small>
                                    <asp:LinkButton ID="lnkForgot" runat="server">Forgot password?</asp:LinkButton></small>
                            </div>
                            <div class="col text-right">
                                <small>
                                    <asp:LinkButton ID="lnkRegister" runat="server" Style="padding-left: 4em">Not a member yet?</asp:LinkButton></small>
                            </div>
                        </div>
                        <hr class="my-4">
                        <button class="btn btn-lg btn-google btn-block text-uppercase" id="btnGoogle" runat="server"><i class="fa fa-google mr-2"></i>Sign in with Google</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
