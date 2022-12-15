import { Paper } from "@mui/material";
import React from "react";

interface ICommandInfoProps {
  commandGroup: ICommandGroup;
}

const CommandInfo = (props: ICommandInfoProps) => {
  return <Paper elevation={2}>/{props.commandGroup.name}</Paper>;
};
export default CommandInfo;
