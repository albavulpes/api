import * as HttpStatus from "http-status-codes";

import {JsonController, Get, Res, Patch, Req, UseBefore, Param} from "routing-controllers";
import {authorize} from "../../middlewares/Authorize";
import {SessionManager} from "../../../database/SessionManager";
import {User} from "../../../database/models/auth/User";

@JsonController("/account")
@UseBefore(authorize())
export class AccountController
{
    @Get("/:userId")
    public async getUserById(@Res() response, @Param("userId") userId: string)
    {
        // Get the requested user from the db
        var session = SessionManager.createSession();

        var user = await session.query(User).findOne({_id: userId}).asPromise();

        session.close();

        // Check if user exists
        if (!user)
            return response.sendStatus(HttpStatus.NOT_FOUND);

        return {
            success: true,
            data: user
        };
    }

    @Patch("/:userId")
    public async updateUser(@Req() request, @Res() response, @Param("userId") userId: string)
    {
        // If the user requested is not the current user, send 401
        if (userId !== request.user.id)
            return response.sendStatus(HttpStatus.UNAUTHORIZED);

        // User checked out, let's update the user's data
        var session = SessionManager.createSession();

        var user = await session.query(User).findOne({_id: userId}).asPromise();

        // TODO: Figure out what fields to update
        // Update the user
        Object.assign(user, request.body);

        session.close();

        return {
            success: true,
            data: user
        };
    }
}