import axios from "axios";

const baseUrl = "/api/Commands";

const getCommandGroups = async () => {
  const response = await axios.get(`${baseUrl}/CommandGroups`);
  return response.data as ICommandGroup[];
};

const getCommandGroupById = async (id: number) => {
  const response = await axios.get(`${baseUrl}/CommandGroups/${id}`);
  return response.data as ICommandGroup;
};

const getSubCommandById = async (id: number) => {
  const response = await axios.get(`${baseUrl}/SubCommands/${id}`);
  return response.data as ISubCommand;
};

export default { getCommandGroups, getCommandGroupById, getSubCommandById };
