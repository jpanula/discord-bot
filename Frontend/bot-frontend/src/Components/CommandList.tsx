import React, { useEffect, useState } from "react";
import commandService from "../Services/commands";
import { Box, Paper, Stack } from "@mui/material";
import CommandInfo from "./CommandInfo";

const CommandList = () => {
  const [commandGroups, setCommandGroups] = useState<ICommandGroup[]>([]);

  useEffect(() => {
    const getCommandGroups = async () => {
      setCommandGroups([...(await commandService.getCommandGroups())]);
    };
    void getCommandGroups();
  }, []);

  const commandGroupElements = commandGroups.map(
    (commandGroup): React.ReactElement => (
      <CommandInfo key={commandGroup.id} commandGroup={commandGroup} />
    )
  );

  return (
    <Box sx={{ width: "100%" }}>
    <Stack spacing={2}>
      <Paper elevation={1}>{commandGroupElements}</Paper>
    </Stack>
    </Box>
  );
};
export default CommandList;
