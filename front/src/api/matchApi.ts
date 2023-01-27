import axios from "axios";
import {Hero} from "../models/Hero";
import {heroListPath, matchPath} from "./apiPaths";
import {Match} from "../models/Match";
import {MatchFilterModel} from "../models/filterModels/matchFilterModel";
import {api} from "./http";

export async function getMatch(matchId: Guid): Promise<Match> {
    const response = await api.post<Match>(matchPath, matchId);
    return response.data;
}

export async function getAllMatches(): Promise<Match[]> {
    const response = await api.get<Match[]>(matchPath);
    return response.data;
}

export async function getFilteredMatches(matchFilter: MatchFilterModel): Promise<Match[]> {
    const response = await api.post<Match[]>(`${matchPath}/filter`, matchFilter);
    return response.data;
}

