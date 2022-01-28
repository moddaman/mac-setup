# Working with Pulumi from local CLI

These are instructions for using Pulumi from local cli using AWS SSO and the Starship prompt.

<aside>
💡 This is **Magnus Haugaasen's** setup for his Mac OSX command prompt. If you have other preferences that is okay. In that case replace the relevant steps for your command prompt or OS. You do You 🙂

</aside>

The end goal is to have a prompt that looks like this:

![Untitled](Working%20with%20Pulumi%20from%20local%20CLI%2019afa0a1620647ff9777ef06655cc892/Untitled.png)

1. Install OhMyZSH shell to replace the default Mac OSX shell. Follow the install instructions:
[https://ohmyz.sh/](https://ohmyz.sh/)
(If you are on Windows you might have to use Windows Subsystem for Linux 2 to get such a nice shell, unless there are some new shells for Windows unknown to me)
2. Install Starship:
[https://starship.rs/](https://starship.rs/)
Starship init script must be added to `~/.zshrc`
*Github, NodeJS, Pulumi, and AWS are default modules in Starship*
3. Setup SSO via AWS CLI
[https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-sso.html](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-sso.html)
When configuring SSO these are the values you need:
(for the ones not listed use the default value)
**SSO start URL:** `https://easee.awsapps.com/start#/`
**SSO Region**: `eu-west-1`
**CLI profile name**: choose a value that corresponds with the environment you will use, for this example use `easee-beta`
4. Install `aws-sso-creds` helper to get AWS SSO session tokens into environment variables
‣
5. Create aliases in `~/.zshrc` for each environment / stack to work with. 
Example for beta:

```bash
alias beta='aws sso login --profile easee-beta && export AWS_PROFILE=easee-beta-sso && pulumi stack select easee/beta && eval $(aws-sso-creds export)'
```

1. Login via SSO, set the profile name appropriately such as `easee-beta`on first-itme (we will use this later). 
2. With the `easee-beta` alias already added we can use this for subsequent logins into AWS SSO via CLI
3. Finally call the `beta` on the CLI prompt alias to set the correct AWS session tokens and select the correct Pulumi stack.

### Limiting the AWS accounts Pulumi can work with

If you try to deploy a Pulumi stack onto the wrong AWS environment you would most likely notice that there will be a brutal list of changes in the preview step and you will probably realize what you are about to do and then abort the operation.

But in case you have a sudden case of welders-blindness it would be nice to add a safeguard from creating a total disaster and the cleanup-job of the century.

Pulumi config has a property for limiting the AWS accounts you can work with. Either by explicitly allowing accounts or explicitly denying accounts. You can only have one of these properties.

So adding this to a `Pulumi.beta.yaml` config file will only allow us to deploy this stack to the `easee-beta` account:

![Untitled](Working%20with%20Pulumi%20from%20local%20CLI%2019afa0a1620647ff9777ef06655cc892/Untitled%201.png)

This applies both on local CLI and on build server. Pulumi will now abort any `preview` or `up` operation if the AWS account in context is different.

The same can be done for the `Pulumi.production.yaml` file that most stacks have.

Full reference on the config properties

[https://www.pulumi.com/registry/packages/aws/installation-configuration/#configuration-options](https://www.pulumi.com/registry/packages/aws/installation-configuration/#configuration-options)