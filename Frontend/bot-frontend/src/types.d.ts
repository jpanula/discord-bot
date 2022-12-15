interface ICommandGroup {
  id: number;
  name: string;
  description?: string;
  isConfigurable: boolean;
  subcommands: ISubCommand[];
}

interface ISubCommand {
  id: number;
  name: string;
  description?: string;
  exampleText?: string;
  exampleMediaUrl?: string;
  parameters: ICommandParameter[];
}

interface ICommandParameter {
  id: number;
  name: string;
  description: string;
}
